using ParkShareIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class MasterAddress
    {
        public int Id { get; set; }
        [Required]
        public string JsonString { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(minimum: 0, maximum: 5, ErrorMessage = "")]
        public int NoOfPictures { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}?$", ErrorMessage = "Invalid Zip")]
        public string ZipCode { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [ForeignKey("ApplicationUser")]
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        //public ICollection<AddSpace> addSpaces { get; set; }
    }
}
