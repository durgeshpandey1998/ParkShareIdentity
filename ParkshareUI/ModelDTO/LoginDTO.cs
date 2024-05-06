namespace ParkshareUI.ModelDTO
{
    public class LoginDTO
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime accessTokenValidateTo { get; set; }
        public DateTime refreshTokenValidateTo { get; set; }
    }
}
