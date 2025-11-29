using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/persons")]
public class PersonController : ControllerBase
{
    private readonly PersonService _service;
    private readonly IMapper _mapper;

    public PersonController(PersonService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // POST: api/person
    [HttpPost]
    public async Task<ActionResult<PersonReadDto>> Create(PersonCreateDto dto)
    {
        var person = _mapper.Map<Person>(dto);
        await _service.CreateAsync(person);

        var response = _mapper.Map<PersonReadDto>(person);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    // GET: api/person
    [HttpGet]
    public async Task<ActionResult<PagedResult<PersonReadDto>>> GetPaged(
        int page = 1,
        int pageSize = 10,
        string? search = null,
        string? sort = "Name",
        string? dir = "asc")
    {
        var result = await _service.GetPagedAsync(page, pageSize, search, sort, dir);

        return Ok(new PagedResult<PersonReadDto>
        {
            TotalItems = result.TotalItems,
            Page = result.Page,
            PageSize = result.PageSize,
            Items = _mapper.Map<IEnumerable<PersonReadDto>>(result.Items)
        });
    }

    // GET: api/person/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonReadDto>> GetById(int id)
    {
        var person = await _service.GetByIdAsync(id);

        if (person == null)
            return NotFound();

        return Ok(_mapper.Map<PersonReadDto>(person));
    }

    // PUT: api/person/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, PersonCreateDto dto)
    {
        var existing = await _service.GetByIdAsync(id);

        if (existing == null)
            return NotFound();

        _mapper.Map(dto, existing); // actualiza propiedades sobre la entidad

        await _service.UpdateAsync(existing);
        return NoContent();
    }

    // DELETE: api/person/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
