using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ParkShareIdentity.DTO
{
    public class ImagesViewModelDTO
    {       
        [Required]
        public ICollection<IFormFile> Pictures { get; set; }
        [Required]
        public int MasterAddressId { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
        [Required]
        public ICollection<bool> IsPreview { get; set; }
        [Required]
        public ICollection<int> Order { get; set; }
     
    }
}
