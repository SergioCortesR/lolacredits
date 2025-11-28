using Microsoft.EntityFrameworkCore;

public class InstallmentService
{
    private readonly AppDbContext _db;

    public InstallmentService(AppDbContext db)
    {
        _db = db;
    }

    // Get installments by LoanId
    public async Task<List<Installment>> GetByLoanIdAsync(int loanId)
    {
        return await _db.Installments
            .Where(i => i.LoanId == loanId)
            .Include(i => i.Payments)
            .OrderBy(i => i.PeriodNumber)
            .ToListAsync();
    }

    // Get installment by ID
    public async Task<Installment?> GetByIdAsync(int id)
    {
        return await _db.Installments
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    // Get pending installments for a loan
    public async Task<List<Installment>> GetPendingByLoanIdAsync(int loanId)
    {
        return await _db.Installments
            .Where(i => i.LoanId == loanId && i.Status != InstallmentStatus.Paid)
            .Include(i => i.Payments)
            .OrderBy(i => i.PeriodNumber)
            .ToListAsync();
    }
}
