namespace ParkShareIdentity.Shared.Interface
{
    public interface ISwaggerResponse
    {
         string InternalServerError500 { get { return "Internal Server Error "; } }
         string UnAuthorized401 { get { return "API Not Authorized "; } }

         string BadRequest400 { get { return "User Not Found "; } }
    }
}
