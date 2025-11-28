using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/installments")]
public class InstallmentController : ControllerBase
{
    private readonly InstallmentService _service;
    private readonly IMapper _mapper;

    public InstallmentController(InstallmentService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // GET: api/installment/loan/5
    [HttpGet("loan/{loanId:int}")]
    public async Task<ActionResult<List<InstallmentReadDto>>> GetByLoanId(int loanId)
    {
        var installments = await _service.GetByLoanIdAsync(loanId);
        return Ok(_mapper.Map<List<InstallmentReadDto>>(installments));
    }

    // GET: api/installment/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<InstallmentReadDto>> GetById(int id)
    {
        var installment = await _service.GetByIdAsync(id);

        if (installment == null)
            return NotFound(new { message = "Installment not found" });

        return Ok(_mapper.Map<InstallmentReadDto>(installment));
    }

    // GET: api/installment/pending/loan/5
    [HttpGet("pending/loan/{loanId:int}")]
    public async Task<ActionResult<List<InstallmentReadDto>>> GetPendingByLoanId(int loanId)
    {
        var installments = await _service.GetPendingByLoanIdAsync(loanId);
        return Ok(_mapper.Map<List<InstallmentReadDto>>(installments));
    }
}
