namespace ParkshareUI.ModelDTO
{
    public class GetAllParkingSpaceDTO
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string Zipcode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsVacant { get; set; }
        public double Price { get; set; }
        public string UserName { get; set; }
        public int ParkingSpaceAvailability { get; set; }
        public ICollection<ImageDTO> imagesList { get;  set; }
        public ICollection<TimeWiseDataDTO> timewisedataList { get;  set; }

    }
}
