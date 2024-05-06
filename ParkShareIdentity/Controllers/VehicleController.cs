using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Service.Interface;

namespace ParkShareIdentity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        #region Dependency Injencution
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration _configuration;
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        private readonly IEmailSender _emailSender;
        private readonly IRegisterService _registerService;
        private readonly ParkShareIdentityContext _parkShareIdentityContext;
        private readonly IVehiclePlates _vehiclePlates;

        public VehicleController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender,
             IRegisterService registerService,
             ParkShareIdentityContext parkShareIdentityContext,
            IVehiclePlates vehiclePlates)

        {

            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _registerService = registerService;
            _parkShareIdentityContext = parkShareIdentityContext;
            _vehiclePlates = vehiclePlates;

        }
        #endregion

        [HttpPost]
        [Route("AddVehiclePlates")]
        public async Task<ResponseDataDTO> AddVehiclePlates([FromForm] VehicleDTO vehicleDTO)
        {
            ResponseDataDTO responseData = new ResponseDataDTO();

            try
            {
                if (vehicleDTO != null)
                {
                    var loginUser = User.Identity.Name;
                    var result = await _vehiclePlates.AddVehiclePlate(vehicleDTO,loginUser);
                    responseData.StatusCode = result.StatusCode;
                    responseData.data = result.data;
                    responseData.message = result.message;
                    return responseData;
                }
                else
                {
                    responseData.message = "Can not add empty field.";
                    responseData.StatusCode = StatusCodes.Status406NotAcceptable;
                    return responseData;
                }
            }
            catch (Exception e)
            {
                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.message = e.Message;
                return responseData;
            }
        }

        [HttpPut]
        [Route("UpdateVehiclePlates")]
        public async Task<ResponseDataDTO> UpdateVehiclePlates(VehicleUpdateDTO vehicleDTO)
        {
            ResponseDataDTO responseData = new ResponseDataDTO();
            try
            {

                if (vehicleDTO.VehicleId > 0 && vehicleDTO.NumberPlate!=null )
                {
                    var result = await _vehiclePlates.UpdateVehiclePlates(vehicleDTO);
                    responseData.StatusCode = result.StatusCode;
                    responseData.data = result.data;
                    responseData.message = result.message;
                    return responseData;
                }
                else
                {

                    responseData.message = "Can not add empty field.";
                    responseData.StatusCode = StatusCodes.Status406NotAcceptable;
                    return responseData;
                }
            }
            catch (Exception ex)
            {

                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.message = ex.Message;
                return responseData;
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<ResponseDataDTO> Delete(int Vid)
        {
            ResponseDataDTO responseData = new ResponseDataDTO();
            try
            {
                if (Vid > 0 && Vid != 0)
                {
                    var result = await _vehiclePlates.DeleteVehicles(Vid);
                    responseData.StatusCode = result.StatusCode;
                    responseData.data = result.data;
                    responseData.message = result.message;
                    return responseData;
                }
                else
                {
                    responseData.message = "Can not Delete empty field.";
                    responseData.StatusCode = StatusCodes.Status406NotAcceptable;
                    return responseData;
                }
            }
            catch (Exception ex)
            {

                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.message = ex.Message;
                return responseData;
            }
        }

        [HttpGet]
        [Route("GetVehicleById")]
        public async Task<ResponseDataDTO> GetVehicleById(int vehicleId)
        {
            ResponseDataDTO responseData = new ResponseDataDTO();
            try
            {

                if (vehicleId != 0)
                {
                    var result = await _vehiclePlates.GetVehicleById(vehicleId);
                    responseData.StatusCode = result.StatusCode;
                    responseData.data = result.data;
                    responseData.message = result.message;
                    return responseData;

                }
                else
                {
                    responseData.message = "Can not Find empty field.";
                    responseData.StatusCode = StatusCodes.Status406NotAcceptable;
                    return responseData;
                }
            }
            catch (Exception ex)
            {

                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.message = ex.Message;
                return responseData;
            }
        }


        [HttpGet]
        [Route("GetVehicleByUserId")]
        public async Task<ResponseDataDTO> GetVehicleByUserId()
        {
            ResponseDataDTO responseData = new ResponseDataDTO();
            try
            {
                var CurrentUser = User.Identity.Name;
               
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                if (CurrentUser != null)
                {
                    var vehicle = await _vehiclePlates.GetVehicleByUserId(CurrentUser);
                    responseData.StatusCode = vehicle.StatusCode;
                    responseData.data = vehicle.data;
                    responseData.message = vehicle.message;
                    return responseData;
                }
                else
                {
                    responseData.message = "Can not Find empty field.";
                    responseData.StatusCode = StatusCodes.Status406NotAcceptable;
                    return responseData;
                }
            }
            catch (Exception ex)
            {

                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.message = ex.Message;
                return responseData;
            }
        }
         [Authorize] 
        [HttpGet]
        [Route("VehicleList")]
        public async Task<ResponseDataDTO> GetAllVehicle()
        {
            ResponseDataDTO responseData = new ResponseDataDTO();
            try
            {
                var vehicle = await _vehiclePlates.GetAllVehicle();
                if (vehicle != null)
                {
                    responseData.StatusCode = vehicle.StatusCode;
                    responseData.data = vehicle.data;
                    responseData.message = vehicle.message;
                    return responseData;
                }
                else
                {
                    responseData.message = "No Record Found.";
                    responseData.StatusCode = StatusCodes.Status406NotAcceptable;
                    return responseData;
                }

            }
            catch (Exception ex)
            {
                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.message = ex.Message;
                return responseData;
            }
        }
    }



}

