using System.ComponentModel.DataAnnotations;

namespace ParkShareIdentity.Model
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
