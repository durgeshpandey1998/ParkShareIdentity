using Microsoft.AspNetCore.Mvc;

namespace ParkShareIdentity.Response
{
    public class ReturnType
    {
        public int StatusCode;
        public string Message;
    }
    public class DynamicResponse : Controller
    {
        public DynamicResponse()
        {
            int a = 200;
        }
        public JsonResult Ok(string message)
        {
            var jsonData = new
            {
                message = message
            };
            return new JsonResult(jsonData);
        }

        public ReturnType Ok(int statusCode, string message)
        {
            var jsonData = new
            {
                StatusCode = statusCode,
                Message = message
            };
            //return new JsonResult(jsonData);
            ReturnType returnType = new ReturnType();
            returnType.StatusCode = statusCode;
            returnType.Message = message;

            return returnType;
        }
        public JsonResult Ok(int statusCode, string message, dynamic data)
        {
            var jsonData = new
            {
                StatusCode = statusCode,
                Message = message,
                data = data
            };
            return new JsonResult(jsonData);
        }


    }
}
