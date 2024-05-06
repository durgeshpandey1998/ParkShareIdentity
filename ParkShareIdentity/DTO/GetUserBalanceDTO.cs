namespace ParkShareIdentity.DTO
{
    public class GetUserBalanceDTO
    {
        public int WalletId { get; set; }
        public double WalletBalance { get; set; }
        public string UserName { get; set; }
        public DateTime? ActiveDate { get; set; }
        public bool Status { get; set; }

    }
}
