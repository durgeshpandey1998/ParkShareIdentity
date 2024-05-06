using System.ComponentModel.DataAnnotations;

namespace ParkshareUI.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Can't Enter more than 50 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password Contain 6 Characters include number 1 special character & 1 Uppercase Letter.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name Contain only Letters.")]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        [StringLength(25, ErrorMessage = "Can't Enter more than 25 characters")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "last name is required")]
        //[DataType(DataType.Text)]
        //[Display(Name = "LastName")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        //public string LastName { get; set; }

        [Required(ErrorMessage = "City is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name Contain only Letters.")]
        [StringLength(30, ErrorMessage = "Can't Enter more than 30 characters")]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }


        [Required(ErrorMessage = "ZipCode is Required")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Zip code enter in 5 characters")] //^[0-9]{5}(?:-[0-9]{4})?$
        [DataType(DataType.Text)]
        [Display(Name = "ZIPCode")]
        public int ZIPCode { get; set; }

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(25, ErrorMessage = "Can't Enter more than 25 characters")]
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
