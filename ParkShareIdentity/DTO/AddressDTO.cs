using System.ComponentModel.DataAnnotations;

namespace ParkShareIdentity.DTO
{
    public class AddressDTO
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "UserName")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string UserName { get; set; }
        [Required]
        // [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 25)]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ZIPCode")]
        public int ZIPCode { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "latitude")]
        public string? latitude { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "longitude")]
        public string? longitude { get; set; }

    }
}
