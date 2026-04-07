using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isCustomer = User.IsInRole("Customer");
                await _transferService.ExecuteAsync(request, userId, isCustomer, cancellationToken);
                return Ok(new { message = "Transfer completed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isCustomer = User.IsInRole("Customer");
                var transfers = await _transferService.GetAllAsync(userId, isCustomer, cancellationToken);
                return Ok(transfers);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
