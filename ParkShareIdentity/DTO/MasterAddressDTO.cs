using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ParkShareIdentity.DTO
{
    public class MasterAddressDTO
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [Required]
        public string JsonString { get; set; }
        //[RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        [RegularExpression(@"^\d{4}?$", ErrorMessage = "Invalid Zip")]
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public string Description { get; set; }
        [JsonIgnore]        
        public int? NoOfPictures { get; set; } = 0;
        [JsonIgnore]
        public string? UserId { get; set; }
        public MasterAddressDTO()
        {
            NoOfPictures = 0;
        }
    }
}
