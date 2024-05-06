using ParkShareIdentity.Areas.Identity.Data;

namespace ParkShareIdentity.DTO
{
    public class ParkingSpaceDistanceDTO
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public bool IsVacant { get; set; }
        public double Price { get; set; }
        public string UserName { get; set; }
        public int ParkingSpaceAvailability { get; set; }
        public string? ImageUrl { get; set; }
        public List<Images> ImagesList { get; internal set; }
        public List<TimeWiseData> TimewisedataList { get; internal set; }
        public List<AddBooking> BookedDataList { get; internal set; }
        public bool ParkingSpaceStatus { get; set; }
        public ParkingSpaceDistanceDTO()
        {
            ParkingSpaceStatus=false;
        }
    }
}
