public class Installment
{
    public int Id { get; set; }

    public int LoanId { get; set; }
    public Loan Loan { get; set; } = null!;

    public int PeriodNumber { get; set; }           // 1,2,3...
    public DateOnly ExpectedPaymentDate { get; set; }
    public decimal ExpectedAmount { get; set; }     // quota for this month
    public decimal PaidAmount { get; set; }         // sum of payments
    public string Status { get; set; } = "Pending"; // Pending, Partial, Paid

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
