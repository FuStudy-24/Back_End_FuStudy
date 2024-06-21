using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FuStudy_Service.Service;

namespace FuStudy_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
    {
        try
        {
            var result = await _paymentService.CreatePaymentLink(
                request.OrderId,
                request.Amount,
                request.Description
            );

            return Ok(new
            {
                Success = true,
                Result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Success = false, Message = ex.Message });
        }
    }
}

public class CreatePaymentRequest
{
    public int OrderId { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
}