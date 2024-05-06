using System.Text.Json.Serialization;

namespace ParkShareIdentity.DTO
{
    public class BookParkingSpacesDTO
    {
        [JsonIgnore]
        public DateOnly? Date { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
    }

    public class BookParkingSpacesBasedOnPlaceAndDateDTO
    {
        public DateTime? Date { get; set; }
        public List<ManageDateDTO>? DateTime1 { get; set; }
        public double? Lattitude { get; set; }
        public double? Longitude { get; set; }
    }
}
