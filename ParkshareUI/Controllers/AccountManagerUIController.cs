using Microsoft.AspNetCore.Mvc;
using ParkshareUI.ModelDTO;
using ParkshareUI.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ParkshareUI.Controllers
{
    public class AccountManagerUIController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(Register registerModel)
        {
            // Initialization.  
            // DataTable responseObj = new DataTable();
            var data = "";
            string result = "";
            // Posting.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri("https://localhost:44347/");

                // Setting content type.                   
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP POST  
                response = await client.PostAsJsonAsync("api/AccountManager/Registration", registerModel).ConfigureAwait(false);


                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                    result = response.Content.ReadAsStringAsync().Result;
                    // data = (string)JsonConvert.DeserializeObject(result);
                }
            }

            //return responseObj;
            ViewBag.Message = string.Format("Hello {0}.\\nCurrent Date and Time: {1}", result, DateTime.Now.ToString());

            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            // Initialization.  
            // DataTable responseObj = new DataTable();
            var data = "";
            string result = "";
            // Posting.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri("https://localhost:44347/");

                // Setting content type.                   
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP POST  
                response = await client.PostAsJsonAsync("api/AccountManager/Login", login).ConfigureAwait(false);
                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == (System.Net.HttpStatusCode)200)
                    {
                        // Reading Response.  
                        result = response.Content.ReadAsStringAsync().Result;
                        var myData = JsonSerializer.Deserialize<ResponseDTO>(result);
                        string? test = myData.data.ToString();
                        if (myData.statusCode == 200)
                        {
                            LoginDTO loginDTO = new LoginDTO();
                            var deserilized = JsonSerializer.Deserialize<LoginDTO>(test);


                            loginDTO.accessToken = deserilized.accessToken;
                            loginDTO.refreshToken = deserilized.refreshToken;
                            loginDTO.refreshTokenValidateTo = deserilized.refreshTokenValidateTo;
                            loginDTO.accessTokenValidateTo = deserilized.accessTokenValidateTo;
                            
                           TempData["mydata"] = test;
                            return RedirectToAction("Index", "DashBoard");
                    }

                }
                else
                {
                    ViewBag.data = result;

                    return View();
                }

            }
        }

        //return responseObj;
        //ViewBag.Message = string.Format("Hello {0}.\\nCurrent Date and Time: {1}", result,DateTime.Now.ToString());
        ViewBag.data = result;

            return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllParkingSpace()
    {
        // Initialization.  
        // DataTable responseObj = new DataTable();
        var data = "";
        string result = "";
        // Posting.  
        using (var client = new HttpClient())
        {
            string authorization = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZHBkQG5hcm9sYS5lbWFpbCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2dpdmVubmFtZSI6ImRwZEBuYXJvbGEuZW1haWwiLCJqdGkiOiI0NWIwYWY4YS04MGRmLTRhMDktYTg2MC0zZTJjZmVkYjI3YTUiLCJleHAiOjE2NzU5MjE1NDYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NzE0NCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NzE0NCJ9.UbiZcfC71ivMcKFNn2siuAxdxPcGPFoMkaeiKQz1tPo";

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
            //  response = await client.PostAsJsonAsync("api/AccountManager/Login", login).ConfigureAwait(false);
            response = await client.GetAsync(test).ConfigureAwait(false);
            //response = await client.GetAsync($"{test}");


            // Verification  
            if (response.IsSuccessStatusCode)
            {
                // Reading Response.  
                result = response.Content.ReadAsStringAsync().Result;
                // data = (string)JsonConvert.DeserializeObject(result);
            }
        }

        //return responseObj;
        //ViewBag.Message = string.Format("Hello {0}.\\nCurrent Date and Time: {1}", result,DateTime.Now.ToString());
        ViewBag.data = result;
        return View();
    }
}
}
