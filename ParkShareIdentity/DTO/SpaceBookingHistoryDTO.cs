namespace ParkShareIdentity.DTO
{
    public class SpaceBookingHistoryDTO
    {
        public string ParkingSpaceOwner { get; set; }
        public string Zipcode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public double Lattittude { get; set; }
        public double Longitude { get; set; }
        public bool IsVacant { get; set; }
        public int ParkingSpaceAvailability { get; set; }
        public double TotalPrice { get; set; }
        public TimeSpan TotalHours { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string ParkingSpaceRenter { get; set; }
    }
}
