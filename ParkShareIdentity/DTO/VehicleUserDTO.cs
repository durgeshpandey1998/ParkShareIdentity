using ParkShareIdentity.Data;

namespace ParkShareIdentity.DTO
{
    public class VehicleUserDto
    {
        public int VehicleId { get; set; }
        public string UserName { get; set; }
        public string? VehicleType { get; set; }
        public string licencensePlateNumber { get; set; }

    }
}
