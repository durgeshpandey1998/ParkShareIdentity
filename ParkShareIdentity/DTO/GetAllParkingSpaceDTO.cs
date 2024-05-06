namespace ParkShareIdentity.DTO
{
    public class GetAllParkingSpaceDTO
    {
        public int Id { get; set; }
        public string Zipcode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsVacant { get; set; }
        public int ParkingSpaceAvailability { get; set; }
        public double Price { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public string UserName { get; set; }
    }
}
