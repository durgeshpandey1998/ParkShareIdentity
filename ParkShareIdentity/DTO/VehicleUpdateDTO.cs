using System.ComponentModel.DataAnnotations;

namespace ParkShareIdentity.DTO
{
    public class VehicleUpdateDTO
    {
        public int VehicleId { get; set; }
    
        [Required(ErrorMessage = "Vehicle Type is Required.")]
        public string VehicleType { get; set; }

        [Required(ErrorMessage = "NumberPlate is Required.")]

        public string? NumberPlate { get; set; }
    }
}
