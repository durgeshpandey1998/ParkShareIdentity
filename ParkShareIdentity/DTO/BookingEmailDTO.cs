namespace ParkShareIdentity.DTO
{
    public class BookingEmailDTO
    {
        public string ParkingOwner { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public double TotalPrice { get; set; }
        DateTime ? FromDate { get; set; } 
        DateTime ? ToDate { get; set; }
    }
}
