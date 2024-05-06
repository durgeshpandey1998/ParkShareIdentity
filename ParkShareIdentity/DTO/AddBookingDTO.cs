namespace ParkShareIdentity.DTO
{
    public class AddBookingDTO
    {

        public int VehicleId { get; set; }

        public int AddSpaceId { get; set; }
        public string UserId { get; set; }
        public int TimewiseId {get;set;}
        public DateTime? date1 { get; set; }
        public List<ManageDateDTO>? DateTime1 { get; set; }


    }
}
