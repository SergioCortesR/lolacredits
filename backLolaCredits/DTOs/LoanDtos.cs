public class LoanCreateDto
{
    public int PersonId { get; set; }
    public required decimal Amount { get; set; }
    public int Months { get; set; }
    public int InterestRate { get; set; }
    public int PaymentDay { get; set; }
    public DateOnly? LoanDate { get; set; } // Si es null, se usará la fecha actual
}

public class LoanReadDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public required string PersonName { get; set; }
    public string PersonEmail { get; set; } = string.Empty;
    public string PersonCI { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Months { get; set; }
    public int InterestRate { get; set; }
    public int PaymentDay { get; set; }
    public decimal MonthlyAmount { get; set; }
    public DateOnly LoanDate { get; set; }
    public DateOnly DueDate { get; set; }
    public int PaidInstallments { get; set; }
    public int PendingInstallments { get; set; }
}

public class LoanDetailDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public required string PersonName { get; set; }
    public decimal Amount { get; set; }
    public int Months { get; set; }
    public int InterestRate { get; set; }
    public int PaymentDay { get; set; }
    public decimal MonthlyAmount { get; set; }
    public DateOnly LoanDate { get; set; }
    public DateOnly DueDate { get; set; }
    public List<InstallmentReadDto>? Installments { get; set; }
}
