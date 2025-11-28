using Microsoft.EntityFrameworkCore;

public class PaymentService
{
    private readonly AppDbContext _db;

    public PaymentService(AppDbContext db)
    {
        _db = db;
    }

    // Register a payment for an installment
    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        // 1. Validate: installment must exist
        var installment = await _db.Installments.FindAsync(payment.InstallmentId);
        if (installment == null)
            throw new Exception("Installment not found.");

        // 1.b Prevent paying ahead: ensure all previous periods are paid
        var hasPreviousPending = await _db.Installments
            .AnyAsync(i => i.LoanId == installment.LoanId
                        && i.PeriodNumber < installment.PeriodNumber
                        && i.Status != InstallmentStatus.Paid);
        if (hasPreviousPending)
            throw new Exception("No puede pagar esta cuota hasta que las cuotas anteriores estén pagadas.");

        // 2. Validate: payment amount must be positive
        // Round payment amount to 2 decimals
        payment.Amount = decimal.Round(payment.Amount, 2, MidpointRounding.AwayFromZero);
        if (payment.Amount <= 0)
            throw new Exception("Payment amount must be greater than 0.");

        // 3. Validate: payment amount cannot exceed remaining amount
        // compute remaining with rounding
        decimal remainingAmount = decimal.Round(installment.ExpectedAmount - installment.PaidAmount, 2, MidpointRounding.AwayFromZero);
        if (payment.Amount > remainingAmount)
            throw new Exception($"Payment amount cannot exceed remaining amount: {remainingAmount}");

        // 4. Set payment date if not provided
        if (payment.PaymentDate == default)
            payment.PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        // 5. Save payment and update installment inside a transaction
        using (var tx = await _db.Database.BeginTransactionAsync())
        {
            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            // 6. Update installment paid amount and status (apply rounding)
            installment.PaidAmount = decimal.Round(installment.PaidAmount + payment.Amount, 2, MidpointRounding.AwayFromZero);

            if (installment.PaidAmount >= decimal.Round(installment.ExpectedAmount, 2, MidpointRounding.AwayFromZero))
                installment.Status = InstallmentStatus.Paid;
            else if (installment.PaidAmount > 0)
                installment.Status = InstallmentStatus.Partial;

            _db.Installments.Update(installment);
            await _db.SaveChangesAsync();

            await tx.CommitAsync();
        }

        return payment;
    }

    // Get all payments for an installment
    public async Task<List<Payment>> GetByInstallmentIdAsync(int installmentId)
    {
        return await _db.Payments
            .Where(p => p.InstallmentId == installmentId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    // Get payment by ID
    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _db.Payments.FirstOrDefaultAsync(p => p.Id == id);
    }

    // Get all payments for a loan (through installments)
    public async Task<List<Payment>> GetByLoanIdAsync(int loanId)
    {
        return await _db.Payments
            .Where(p => p.Installment.LoanId == loanId)
            .Include(p => p.Installment)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }
}
