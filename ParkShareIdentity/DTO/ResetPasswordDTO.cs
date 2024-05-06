using System.ComponentModel.DataAnnotations;

namespace ParkShareIdentity.Model
{
    public class ResetPasswordDTO
    {
        [Required]
        [EmailAddress]
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

        [Required]
        public string Code { get; set; }
       // public DateTime senttime { get; set; }
       
    }
}
