using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ParkShareIdentity.DTO
{
    public class VehicleDTO
    {
        public int VehicleId { get; set; }
        [Required(ErrorMessage="User Id is Required.")]
        public string VehicleType { get; set; }

        [Required(ErrorMessage = "NumberPlate is Required.")]

        public string? NumberPlate { get; set; }
       
    }
}
