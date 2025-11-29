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
    public async Task<PagedResult<Person>> GetPagedAsync(
    int page,
    int pageSize,
    string? search,
    string? sort,
    string? dir)
    {
        var query = _db.Persons.AsQueryable();

        // Filtro
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(search) ||
                p.LastName.ToLower().Contains(search) ||
                p.SecondLastName.ToLower().Contains(search) ||
                p.Email.ToLower().Contains(search) ||
                p.CI.ToLower().Contains(search)
            );
        }

        // Orden dinámico (por columna)
        if (!string.IsNullOrWhiteSpace(sort))
        {
            query = dir == "desc"
                ? query.OrderByDescending(e => EF.Property<object>(e, sort))
                : query.OrderBy(e => EF.Property<object>(e, sort));
        }

        // Total antes de paginar
        var totalItems = await query.CountAsync();

        // Paginación
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PagedResult<Person>
        {
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }


    // Read by Id
    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _db.Persons
            .Include(p => p.Loans)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Read all
    public async Task<List<Person>> GetAllAsync()
    {
        return await _db.Persons.ToListAsync();
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
