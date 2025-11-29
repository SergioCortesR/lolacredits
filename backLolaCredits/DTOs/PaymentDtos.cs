public class PaymentCreateDto
{
    public int InstallmentId { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}

public class PaymentReadDto
{
    public int Id { get; set; }
    public int InstallmentId { get; set; }
    public decimal Amount { get; set; }
    public DateOnly PaymentDate { get; set; }
    public string? Notes { get; set; }
}
