using ParkShareIdentity.Service;

namespace ParkShareIdentity.Shared
{
    public class SharedUsers
    {
        private readonly IConfiguration _config;
        public IConfiguration _configuration;
        public SharedUsers() { }
        public SharedUsers(IConfiguration config, IConfiguration configuration)
        {
            _config = config;
            _configuration = configuration;
        }
        public dynamic GetUserIDBasedOnUserName(string userName)
        {
            RegisterService registerService = new RegisterService();
            var data = registerService.GetUserIDBasedOnUserName(userName);
            return data;
        }

    }
}
