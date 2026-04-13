using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletOps.Application.DTOs;
using WalletOps.Application.Interfaces;
using System.Security.Claims;

namespace WalletOps.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize(Roles = "SystemAdmin,BankOfficer")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAllAsync(cancellationToken);

            var result = accounts.Select(a => new
            {
                a.Id,
                a.CustomerId,
                a.AccountNumber,
                a.Currency,
                a.Balance,
                a.OverdraftLimit,
                a.Status
            });

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin,BankOfficer")]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = await _accountService.CreateAsync(request, cancellationToken);
                return Created(string.Empty, new { id = accountId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("mine")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyAccounts(CancellationToken cancellationToken)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var accounts = await _accountService.GetMyAccountsAsync(userId!, cancellationToken);
                var result = accounts.Select(a => new
                {
                    a.Id,
                    a.CustomerId,
                    a.AccountNumber,
                    a.Currency,
                    a.Balance,
                    a.OverdraftLimit,
                    a.Status
                });
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
