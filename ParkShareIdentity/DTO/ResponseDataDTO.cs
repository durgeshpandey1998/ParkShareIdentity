using System.ComponentModel.DataAnnotations;

namespace ParkShareIdentity.DTO
{
    public class ResponseDataDTO
    {
        public dynamic StatusCode { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        

    }
}
