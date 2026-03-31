namespace WalletOps.Application.DTOs
{
    public class CreateCustomerRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string? UserId { get; set; }
    }
}
