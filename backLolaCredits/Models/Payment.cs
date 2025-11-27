public class Payment
{
    public int Id { get; set; }

    public int InstallmentId { get; set; }
    public Installment Installment { get; set; } = null!;

    public DateOnly PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}
