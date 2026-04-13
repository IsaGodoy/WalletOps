namespace WalletOps.Application.DTOs
{
    public class CreateCustomerAccessRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
