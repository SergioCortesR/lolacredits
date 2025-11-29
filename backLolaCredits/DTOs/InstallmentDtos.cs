public class InstallmentReadDto
{
    public int Id { get; set; }
    public int PeriodNumber { get; set; }
    public decimal ExpectedAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateOnly ExpectedPaymentDate { get; set; }
    public InstallmentStatus Status { get; set; } = InstallmentStatus.Pending;
}
