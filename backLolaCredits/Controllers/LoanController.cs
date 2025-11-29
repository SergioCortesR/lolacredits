using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/loans")]
public class LoanController : ControllerBase
{
    private readonly LoanService _service;
    private readonly IMapper _mapper;

    public LoanController(LoanService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // POST: api/loan
    [HttpPost]
    public async Task<ActionResult<LoanReadDto>> Create(LoanCreateDto dto)
    {
        try
        {
            var loan = _mapper.Map<Loan>(dto);
            var created = await _service.CreateLoanAsync(loan);
            var response = _mapper.Map<LoanReadDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/loan
    [HttpGet]
    public async Task<ActionResult<PagedResult<LoanReadDto>>> GetPaged(
        int page = 1,
        int pageSize = 10,
        string? search = null,
        string? sort = "LoanDate",
        string? dir = "desc")
    {
        var result = await _service.GetPagedAsync(page, pageSize, search, sort, dir);

        return Ok(new PagedResult<LoanReadDto>
        {
            TotalItems = result.TotalItems,
            Page = result.Page,
            PageSize = result.PageSize,
            Items = _mapper.Map<IEnumerable<LoanReadDto>>(result.Items)
        });
    }

    // GET: api/loan/all
    [HttpGet("all")]
    public async Task<ActionResult<List<LoanReadDto>>> GetAll()
    {
        var loans = await _service.GetAllAsync();
        return Ok(_mapper.Map<List<LoanReadDto>>(loans));
    }

    // GET: api/loan/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanDetailDto>> GetById(int id)
    {
        var loan = await _service.GetByIdAsync(id);

        if (loan == null)
            return NotFound(new { message = "Loan not found" });

        var response = _mapper.Map<LoanDetailDto>(loan);
        return Ok(response);
    }

    // GET: api/loan/person/5
    [HttpGet("person/{personId:int}")]
    public async Task<ActionResult<List<LoanReadDto>>> GetByPersonId(int personId)
    {
        var loans = await _service.GetByPersonIdAsync(personId);
        return Ok(_mapper.Map<List<LoanReadDto>>(loans));
    }

    // PUT: api/loan/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<LoanReadDto>> Update(int id, LoanCreateDto dto)
    {
        try
        {
            var loan = _mapper.Map<Loan>(dto);
            var updated = await _service.UpdateLoanAsync(id, loan);

            if (updated == null)
                return NotFound(new { message = "Loan not found" });

            var response = _mapper.Map<LoanReadDto>(updated);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/loan/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteLoanAsync(id);

        if (!deleted)
            return NotFound(new { message = "Loan not found" });

        return NoContent();
    }
}
