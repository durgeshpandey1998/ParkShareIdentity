using ParkShareIdentity.Areas.Identity.Data;
using ParkShareIdentity.Data;

namespace ParkShareIdentity.DTO
{
    public class UserDTO
    {

        public UserDTO()
        {
            UserData = new ApplicationUserDTO();
            VehicleData = new VehicleDTO();   
        }
        public ApplicationUserDTO UserData { get; set; }
        public VehicleDTO VehicleData { get; set; }
    }
}
