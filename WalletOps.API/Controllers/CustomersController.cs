using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WalletOps.Application.DTOs;
using WalletOps.Application.Interfaces;
using WalletOps.Infrastructure.Identity;

namespace WalletOps.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SystemAdmin,BankOfficer")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomersController(
            ICustomerService customerService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _customerService = customerService;
            _userManager = userManager;
            _roleManager = roleManager;
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

        [HttpPost("{id:guid}/access")]
        public async Task<IActionResult> CreateAccess(Guid id, [FromBody] CreateCustomerAccessRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    return BadRequest(new { error = "Request cannot be null." });
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new { error = "Email is required." });
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new { error = "Password is required." });
                }

                var customer = await _customerService.GetByIdAsync(id, cancellationToken);

                if (!string.IsNullOrWhiteSpace(customer.UserId))
                {
                    return BadRequest(new { error = "Customer already has access assigned." });
                }

                if (!await _roleManager.RoleExistsAsync("Customer"))
                {
                    return BadRequest(new { error = "Customer role is not configured." });
                }

                var existingUser = await _userManager.FindByEmailAsync(request.Email);

                if (existingUser is not null)
                {
                    return BadRequest(new { error = "A user with the same email already exists." });
                }

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FullName = customer.FullName,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user, request.Password);

                if (!createResult.Succeeded)
                {
                    return BadRequest(new
                    {
                        error = string.Join(" ", createResult.Errors.Select(e => e.Description))
                    });
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);

                    return BadRequest(new
                    {
                        error = string.Join(" ", roleResult.Errors.Select(e => e.Description))
                    });
                }

                try
                {
                    await _customerService.AssignUserAsync(id, user.Id, cancellationToken);

                    return Created(string.Empty, new
                    {
                        customerId = customer.Id,
                        userId = user.Id,
                        email = user.Email,
                        role = "Customer"
                    });
                }
                catch
                {
                    await _userManager.DeleteAsync(user);
                    throw;
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
