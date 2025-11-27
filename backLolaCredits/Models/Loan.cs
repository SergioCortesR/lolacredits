public class Loan
{
    public int Id { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public decimal Amount { get; set; }          // principal
    public DateOnly LoanDate { get; set; }
    public int DueDay { get; set; }              // expected payment day
    public int Months { get; set; }              // total number of installments
    public int MonthlyInterest { get; set; }     // percentage

    public ICollection<Installment> Installments { get; set; } = new List<Installment>();
}
