using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParkshareUI.ModelDTO;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ParkshareUI.Controllers
{
    public class DashBoardController : Controller
    {
          private readonly IHttpContextAccessor _httpContextAccessor;
        public DashBoardController(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public class RootObject
        {
    
            public object imagesList { get;  set; }
            public object timewisedataList { get; set; }
        }
        public async Task<IActionResult> Index()
        {
             
            //LoginDTO data = TempData["mydata"] as LoginDTO;
            var data = TempData["mydata"];
            string? accessTokenData = data.ToString();
            LoginDTO loginDTO = new LoginDTO();
            var deserilized = JsonSerializer.Deserialize<LoginDTO>(accessTokenData);
            loginDTO.accessToken = deserilized.accessToken;
            loginDTO.refreshToken = deserilized.refreshToken;
            loginDTO.refreshTokenValidateTo = deserilized.refreshTokenValidateTo;
            loginDTO.accessTokenValidateTo = deserilized.accessTokenValidateTo;   
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(10);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("Token2", loginDTO.accessToken.ToString(),options);
            ViewBag.message = _httpContextAccessor.HttpContext.Request.Cookies["Token2"] ;

            // Initialization.  
            string result = "";
            // Posting.  
            using (var client = new HttpClient())
            {
                string authorization = _httpContextAccessor.HttpContext.Request.Cookies["Token2"];

                // Setting Authorization.  
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);


                // Setting Base address.  

                client.BaseAddress = new Uri("https://localhost:44347/");
                var test = "api/ParkingSpaceBooking/GetAllParkingSpace";
                // Setting content type.                   
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP POST  

                response = await client.GetAsync(test).ConfigureAwait(false);
       
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
          
          
            var myData = JsonSerializer.Deserialize<ResponseDTO>(result);
            string? test2 = myData.data.ToString();
            //List<GetAllParkingSpaceDTO> ObjOrderList1 = JsonSerializer.Deserialize<List<GetAllParkingSpaceDTO>>(test2);
            List<GetAllParkingSpaceDTO> ObjOrderList = JsonConvert.DeserializeObject<List<GetAllParkingSpaceDTO>>(test2);
            JObject jObject = JObject.Parse(result);
            JToken jUser = jObject["data"];

            return View(ObjOrderList);        }
    }
}
