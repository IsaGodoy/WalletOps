using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletOps.Application.DTOs;
using WalletOps.Application.Interfaces;

namespace WalletOps.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SystemAdmin,BankOfficer,Customer")]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService _transferService;
        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTransferRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _transferService.ExecuteAsync(request, cancellationToken);
                return Ok(new { message = "Transfer completed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
