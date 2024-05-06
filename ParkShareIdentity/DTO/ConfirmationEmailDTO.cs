using System.ComponentModel.DataAnnotations;

namespace ParkShareIdentity.DTO
{
    public class ConfirmationEmailDTO
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string code { get; set; }
    }
}
