using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly PersonService _personService;
    private readonly LoanService _loanService;

    public DashboardController(PersonService personService, LoanService loanService)
    {
        _personService = personService;
        _loanService = loanService;
    }

    // GET: api/dashboard/stats
    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetStats()
    {
        var persons = await _personService.GetAllAsync();
        var loans = await _loanService.GetAllAsync();

        var totalLoaned = loans.Sum(l => l.Amount);
        var totalPending = loans
            .SelectMany(l => l.Installments)
            .Sum(i => i.ExpectedAmount - i.PaidAmount);
        var totalPaid = loans
            .SelectMany(l => l.Installments)
            .Sum(i => i.PaidAmount);

        var activeLoans = loans.Count(l => l.Installments.Any(i => i.Status != InstallmentStatus.Paid));
        var completedLoans = loans.Count(l => l.Installments.All(i => i.Status == InstallmentStatus.Paid));

        return Ok(new DashboardStatsDto
        {
            TotalPersons = persons.Count,
            TotalLoans = loans.Count,
            ActiveLoans = activeLoans,
            CompletedLoans = completedLoans,
            TotalLoaned = totalLoaned,
            TotalPaid = totalPaid,
            TotalPending = totalPending
        });
    }
}

public class DashboardStatsDto
{
    public int TotalPersons { get; set; }
    public int TotalLoans { get; set; }
    public int ActiveLoans { get; set; }
    public int CompletedLoans { get; set; }
    public decimal TotalLoaned { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalPending { get; set; }
}
