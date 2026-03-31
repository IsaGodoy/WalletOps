using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletOps.Application.DTOs;
using WalletOps.Application.Interfaces;

namespace WalletOps.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SystemAdmin,BankOfficer")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var customerId = await _customerService.CreateAsync(request, cancellationToken);
                return Created(string.Empty, new { id = customerId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetAllAsync(cancellationToken);

            var result = customers.Select(a => new
            {
                a.Id,
                a.FullName,
                a.DocumentNumber,
                a.CreatedAtUtc
            });

            return Ok(result);
        }
    }
}
