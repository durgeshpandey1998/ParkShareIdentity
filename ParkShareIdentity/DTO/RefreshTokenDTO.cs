using System.ComponentModel.DataAnnotations;

namespace ParkShareIdentity.DTO
{
    public class RefreshTokenDTO
    {
        [Required(ErrorMessage = "AccessToken is required")]
        public string accesToken { get; set; }
        [Required(ErrorMessage = "RefreshToken is required")]
        public string refreshToken { get; set; }
    }
}
