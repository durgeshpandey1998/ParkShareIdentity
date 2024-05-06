using ParkShareIdentity.Shared.Interface;

namespace ParkShareIdentity.Shared
{
    public class SwaggerResponse : ISwaggerResponse
    {
        public string InternalServerError500 { get { return "Internal Server Error "; } }
        public string UnAuthorized401 { get { return "API Not Authorized "; } }

        public string BadRequest400 { get { return "User Not Found "; } }

      
        
    }
}
