namespace WalletOps.Application.DTOs
{
    public class CreateAccountRequest
    {
        public Guid CustomerId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public int Currency { get; set; }
        public decimal OverdraftLimit { get; set; }
    }
}
