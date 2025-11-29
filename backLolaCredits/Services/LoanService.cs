using Microsoft.EntityFrameworkCore;

public class LoanService
{
    private readonly AppDbContext _db;

    public LoanService(AppDbContext db)
    {
        _db = db;
    }

    // Create Loan with auto-generated installments
    public async Task<Loan> CreateLoanAsync(Loan loan)
    {
        // 1. Validate: person must exist
        var person = await _db.Persons.FindAsync(loan.PersonId);
        if (person == null)
            throw new Exception("Person not found.");

        // 2. Validate months
        if (loan.Months <= 0)
            throw new Exception("Months must be greater than 0.");

        if (loan.Amount <= 0)
            throw new Exception("Amount must be greater than 0.");

        if (loan.PaymentDay < 1 || loan.PaymentDay > 28)
            throw new Exception("PaymentDay must be between 1 and 28.");

        if (loan.LoanDate == default)
            loan.LoanDate = DateOnly.FromDateTime(DateTime.UtcNow);

        loan.Amount = decimal.Round(loan.Amount, 2, MidpointRounding.AwayFromZero);
        
        decimal monthlyInterest = decimal.Round(loan.Amount * (loan.InterestRate / 100), 2, MidpointRounding.AwayFromZero);
        decimal capitalPerMonth = decimal.Round(loan.Amount / loan.Months, 2, MidpointRounding.AwayFromZero);
        loan.MonthlyAmount = decimal.Round(capitalPerMonth + monthlyInterest, 2, MidpointRounding.AwayFromZero);
        loan.DueDate = loan.LoanDate.AddMonths(loan.Months);

        _db.Loans.Add(loan);
        await _db.SaveChangesAsync();

        var installments = GenerateInstallments(loan);
        _db.Installments.AddRange(installments);
        await _db.SaveChangesAsync();

        return loan;
    }

    // Get all loans
    public async Task<List<Loan>> GetAllAsync()
    {
        return await _db.Loans
            .Include(l => l.Person)
            .Include(l => l.Installments)
            .ToListAsync();
    }

    // Get paged loans with search and sort
    public async Task<PagedResult<Loan>> GetPagedAsync(int page, int pageSize, string? search, string? sort, string? dir)
    {
        var query = _db.Loans
            .Include(l => l.Person)
            .Include(l => l.Installments)
            .AsQueryable();

        // Search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(l =>
                l.Person.Name.Contains(search) ||
                l.Person.LastName.Contains(search) ||
                l.Person.SecondLastName.Contains(search) ||
                l.Person.Email.Contains(search) ||
                l.Person.CI.Contains(search)
            );
        }

        // Count total items
        var totalItems = await query.CountAsync();

        // Sorting
        query = (sort?.ToLower(), dir?.ToLower()) switch
        {
            ("amount", "asc") => query.OrderBy(l => l.Amount),
            ("amount", "desc") => query.OrderByDescending(l => l.Amount),
            ("months", "asc") => query.OrderBy(l => l.Months),
            ("months", "desc") => query.OrderByDescending(l => l.Months),
            ("interestrate", "asc") => query.OrderBy(l => l.InterestRate),
            ("interestrate", "desc") => query.OrderByDescending(l => l.InterestRate),
            ("loandate", "asc") => query.OrderBy(l => l.LoanDate),
            ("loandate", "desc") => query.OrderByDescending(l => l.LoanDate),
            _ => query.OrderByDescending(l => l.LoanDate) // Default
        };

        // Pagination
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Loan>
        {
            Items = items,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize
        };
    }

    // Get loan by ID with installments
    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _db.Loans
            .Include(l => l.Person)
            .Include(l => l.Installments)
                .ThenInclude(i => i.Payments)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    // Get loans by PersonId
    public async Task<List<Loan>> GetByPersonIdAsync(int personId)
    {
        return await _db.Loans
            .Where(l => l.PersonId == personId)
            .Include(l => l.Installments)
            .ToListAsync();
    }

    // Update Loan (only non-critical fields)
    public async Task<Loan?> UpdateLoanAsync(int id, Loan updated)
    {
        var loan = await _db.Loans
            .Include(l => l.Installments)
            .FirstOrDefaultAsync(l => l.Id == id);
        
        if (loan == null)
            return null;

        // Only allow updating InterestRate and PaymentDay
        // Amount and Months cannot be changed as they affect installments
        if (updated.InterestRate > 0)
            loan.InterestRate = updated.InterestRate;

        if (updated.PaymentDay > 0 && updated.PaymentDay <= 28)
            loan.PaymentDay = updated.PaymentDay;

        _db.Loans.Update(loan);
        await _db.SaveChangesAsync();

        return loan;
    }

    // Delete Loan (cascade deletes installments and payments)
    public async Task<bool> DeleteLoanAsync(int id)
    {
        var loan = await _db.Loans.FindAsync(id);
        if (loan == null)
            return false;

        _db.Loans.Remove(loan);
        await _db.SaveChangesAsync();
        return true;
    }

    private List<Installment> GenerateInstallments(Loan loan)
    {
        var installments = new List<Installment>();
        
        decimal monthlyInterest = decimal.Round(loan.Amount * (loan.InterestRate / 100), 2, MidpointRounding.AwayFromZero);
        decimal capitalPerMonth = decimal.Round(loan.Amount / loan.Months, 2, MidpointRounding.AwayFromZero);
        decimal monthlyPayment = decimal.Round(capitalPerMonth + monthlyInterest, 2, MidpointRounding.AwayFromZero);

        for (int i = 1; i <= loan.Months; i++)
        {
            var paymentDate = loan.LoanDate.AddMonths(i);
            paymentDate = new DateOnly(paymentDate.Year, paymentDate.Month, 
                Math.Min(loan.PaymentDay, DateTime.DaysInMonth(paymentDate.Year, paymentDate.Month)));

            installments.Add(new Installment
            {
                LoanId = loan.Id,
                PeriodNumber = i,
                ExpectedPaymentDate = paymentDate,
                ExpectedAmount = monthlyPayment,
                PaidAmount = 0,
                Status = InstallmentStatus.Pending
            });
        }

        return installments;
    }
}
