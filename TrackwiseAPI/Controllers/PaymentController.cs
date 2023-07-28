using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json.Linq;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost]
        [Route("AddNewCard")]
        public async Task<IActionResult> AddNewCard( NewCard model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.SelectMany(err => err.Errors[0].ErrorMessage));
                }

                CardPayment response = await _paymentRepository.AddNewCard(model);
                
                return Ok(response);
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpGet]
        [Route("GetVaultedCard/{vaultId}")]
        public async Task<IActionResult> GetVaultedCard([FromRoute] string vaultId)
        {
            try
            {
                JToken result = await _paymentRepository.GetVaultedCard(vaultId);
                return Ok(result?.ToString());
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpGet]
        [Route("QueryTransaction/{payRequestId}")]
        public async Task<IActionResult> QueryTransaction([FromRoute] string payRequestId)
        {
            try
            {
                JToken result = await _paymentRepository.QueryTransaction(payRequestId);
                return Ok(result?.ToString());
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}
