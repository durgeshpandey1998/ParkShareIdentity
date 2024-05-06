using ParkShareIdentity.Data;

namespace ParkShareIdentity.DTO
{
    public class UserProfileDTO
    {
        public UserProfileDTO()
        {
            UserProfile = new RegisterModel();        }
        public  virtual RegisterModel UserProfile { get; set; }
       


    }
}
