using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalletOps.Application.Interfaces;

namespace WalletOps.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var accounts = await _accountRepository.GetAllAsync(cancellationToken);
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
    }
}
