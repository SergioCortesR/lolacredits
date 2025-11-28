public class PersonCreateDto
{
    public required string CI { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string SecondLastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
}

public class PersonReadDto
{
    public int Id { get; set; }
    public required string CI { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
}
