public class Person
{
    public int Id { get; set; }

    public string CI { get; set; } = null!; // unique index
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string SecondLastName { get; set; } = null!;
    public string? Phone { get; set; }
    public string Email { get; set; } = null!;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
