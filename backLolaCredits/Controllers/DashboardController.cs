using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/dashboard/stats
    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetStats()
    {
        var totalPersons = await _db.Persons.CountAsync();
        var totalLoans = await _db.Loans.CountAsync();

        var totalLoaned = await _db.Loans.SumAsync(l => l.Amount);
        var totalPaid = await _db.Installments.SumAsync(i => i.PaidAmount);
        var totalPending = await _db.Installments.SumAsync(i => i.ExpectedAmount - i.PaidAmount);

        var activeLoans = await _db.Loans.CountAsync(l => l.Installments.Any(i => i.Status != InstallmentStatus.Paid));
        var completedLoans = await _db.Loans.CountAsync(l => l.Installments.All(i => i.Status == InstallmentStatus.Paid));

        return Ok(new DashboardStatsDto
        {
            TotalPersons = totalPersons,
            TotalLoans = totalLoans,
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
