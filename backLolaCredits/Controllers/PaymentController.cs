using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _service;
    private readonly IMapper _mapper;

    public PaymentController(PaymentService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // POST: api/payment
    [HttpPost]
    public async Task<ActionResult<PaymentReadDto>> Create(PaymentCreateDto dto)
    {
        try
        {
            var payment = _mapper.Map<Payment>(dto);
            var created = await _service.CreatePaymentAsync(payment);
            var response = _mapper.Map<PaymentReadDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/payment/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaymentReadDto>> GetById(int id)
    {
        var payment = await _service.GetByIdAsync(id);

        if (payment == null)
            return NotFound(new { message = "Payment not found" });

        return Ok(_mapper.Map<PaymentReadDto>(payment));
    }

    // GET: api/payment/installment/5
    [HttpGet("installment/{installmentId:int}")]
    public async Task<ActionResult<List<PaymentReadDto>>> GetByInstallmentId(int installmentId)
    {
        var payments = await _service.GetByInstallmentIdAsync(installmentId);
        return Ok(_mapper.Map<List<PaymentReadDto>>(payments));
    }

    // GET: api/payment/loan/5
    [HttpGet("loan/{loanId:int}")]
    public async Task<ActionResult<List<PaymentReadDto>>> GetByLoanId(int loanId)
    {
        var payments = await _service.GetByLoanIdAsync(loanId);
        return Ok(_mapper.Map<List<PaymentReadDto>>(payments));
    }
}
