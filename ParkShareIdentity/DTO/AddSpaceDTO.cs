using ParkShareIdentity.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ParkShareIdentity.DTO
{
    public class AddSpaceDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
        
        [EnumDataType(typeof(SpaceAvailability), ErrorMessage = "Invalid SpaceAvailability")]
        [Required]
        public int ParkingSpaceAvailablity { get; set; }
        [EnumDataType(typeof(PreviewTimeInWeeks), ErrorMessage = "Invalid PreviewTimeInWeeks")]
        [Required]
        public int PreviewTimeInWeeks { get; set; }        
        [Required]
        [RegularExpression(@"^[0-9]+(?:\.[0-9]{1,1})?$", ErrorMessage = "Price can have only one precision")]
        [Range(0.1, double.PositiveInfinity, ErrorMessage = "Price must be greater than 0.1 CHF")]
        
        public double Price { get; set; }
        //[EnumDataType(typeof(Days), ErrorMessage = "Invalid Days")]
       
        public List<ManageDateDTO>? DateTime1 { get; set; }
        public int? MasterAddressId { get; set; }

    }
}
