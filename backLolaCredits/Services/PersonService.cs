using Microsoft.EntityFrameworkCore;

public class PersonService
{
    private readonly AppDbContext _db;

    public PersonService(AppDbContext db)
    {
        _db = db;
    }

    // Create
    public async Task<Person> CreateAsync(Person person)
    {
        _db.Persons.Add(person);
        await _db.SaveChangesAsync();
        return person;
    }

    // Read all
    public async Task<List<Person>> GetAllAsync()
    {
        return await _db.Persons
            .AsNoTracking()
            .ToListAsync();
    }

    // Read by Id
    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _db.Persons
            .Include(p => p.Loans)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Read by CI
    public async Task<Person?> GetByCIAsync(string ci)
    {
        return await _db.Persons
            .Include(p => p.Loans)
            .FirstOrDefaultAsync(p => p.CI == ci);
    }

    // Update
    public async Task<bool> UpdateAsync(Person person)
    {
        if (!await _db.Persons.AnyAsync(p => p.Id == person.Id))
            return false;

        _db.Persons.Update(person);
        await _db.SaveChangesAsync();
        return true;
    }

    // Delete
    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _db.Persons.FindAsync(id);

        if (existing == null)
            return false;

        _db.Persons.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}
