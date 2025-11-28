public class Loan
{
    public int Id { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public decimal Amount { get; set; }          // principal
    public DateOnly LoanDate { get; set; }
    public DateOnly DueDate { get; set; }              // final payment date
    public int Months { get; set; }              // total number of installments
    public int InterestRate { get; set; }        // percentage
    public int PaymentDay { get; set; }          // day of month for payments (1-28)

    public decimal MonthlyAmount { get; set; }  // calculated
    public ICollection<Installment> Installments { get; set; } = new List<Installment>();
}
