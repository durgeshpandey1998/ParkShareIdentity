using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Response;
using ParkShareIdentity.Service.Interface;
using ParkShareIdentity.Shared;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ParkShareIdentity.Controllers
{
     [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpaceBookingController : ControllerBase
    {
        #region Dependency Injencution
        private readonly IAddSpace _addSpace;
        private readonly IMasterAddress _masterAddress;
        private MasterAddressDTO masterAddressViewModel = new MasterAddressDTO();
        public IConfiguration _configuration;
        private UserManager<ApplicationUser> _userManager;
        DynamicResponse dr = new DynamicResponse();
        ValidateModelState validateModelState = new ValidateModelState();
        private readonly ParkShareIdentityContext _ParkShareIdentityContext;
        private readonly IBookParkingSpace _bookParkingSpace;
        private readonly IServer _server;
        private IHostingEnvironment _Environment;
        public ParkingSpaceBookingController(IAddSpace addSpace, IMasterAddress masterAddress, IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            ParkShareIdentityContext ParkShareIdentityContext, IBookParkingSpace bookParkingSpace, IServer server, IHostingEnvironment Environment)
        {
            _addSpace = addSpace;
            _masterAddress = masterAddress;
            _configuration = configuration;
            _userManager = userManager;
            _ParkShareIdentityContext = ParkShareIdentityContext;
            _bookParkingSpace = bookParkingSpace;
            _server= server;
            _Environment= Environment;
            

    }
        #endregion

        #region Get All Parking Space
        [HttpGet]
        [Route("GetAllParkingSpace")]
        public async Task<ResponseDataDTO> GetAllParkingSpace()
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            try
            {
                 responseDataDTO = await _bookParkingSpace.GetAllParkingSpace();
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                return responseDataDTO;
            }
            catch (Exception ex)
            {
                responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                responseDataDTO.message = ex.Message;
                return responseDataDTO;
            }
        }

        #endregion

        #region Get Parking Space By Filter
        [HttpPost]
        [Route("GetParkSpaceByFilter")]
        public async Task<ResponseDataDTO> GetParkSpaceByFilter([FromBody] BookParkingSpacesBasedOnPlaceAndDateDTO bookParkingSpacesDTO)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            responseDataDTO = await _bookParkingSpace.GetAllParkingSpaceByFilter(bookParkingSpacesDTO);
            return responseDataDTO;
        }

        #endregion

        #region Currently Not In Use

        //[HttpGet]
        //[Route("GetAllParkingSpaceByFilter")]
        //public async Task<ResponseDataDTO> GetAllParkingSpaceByFilter(BookParkingSpacesDTO bookParkingSpacesDTO)
        //{
        //    try
        //    {
        //        var allparkingspaceData = await _bookParkingSpace.GetAllParkingSpaceByFilter(bookParkingSpacesDTO);
        //        // responseDataDTO.data = allparkingspaceData;
        //        // responseDataDTO.StatusCode = StatusCodes.Status200OK;
        //        return allparkingspaceData;
        //    }
        //    catch (Exception ex)
        //    {
        //        responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
        //        responseDataDTO.message = ex.Message;
        //        return responseDataDTO;
        //    }
        //}

        #endregion

        #region Get All booking Detail By User Id
        //[HttpGet]
        //[Route("GetAllBookDetailsByUserId")]
        //public async Task<ResponseDataDTO> GetAllBookDetailsByUserId(string email)
        //{
        //    try
        //    {
        //        var allparkingspaceData = await _bookParkingSpace.GetAllBookingDetailsByUserId(email);
        //        responseDataDTO.data = allparkingspaceData;
        //        responseDataDTO.StatusCode = StatusCodes.Status200OK;
        //        return responseDataDTO;
        //    }
        //    catch (Exception ex)
        //    {
        //        responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
        //        responseDataDTO.message = ex.Message;
        //        return responseDataDTO;
        //    }
        //}
        #endregion

        #region Book Parking Space

        [HttpPost]
        [Route("AddBooking")]
        public async Task<ResponseDataDTO> AddBooking(AddBookingDTO addBookingDTO)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            try
            {
                if (addBookingDTO.DateTime1[0].From <= DateTime.Now && addBookingDTO.DateTime1[0].To <= DateTime.Now)
                {
                    responseDataDTO.StatusCode = StatusCodes.Status406NotAcceptable;
                    responseDataDTO.message = "date time not valid";
                    return responseDataDTO;
                }
                var loginUser = User.Identity.Name;
                responseDataDTO = await _bookParkingSpace.AddBooking(addBookingDTO, loginUser);
                return responseDataDTO;

            }
            catch (Exception ex)
            {
                responseDataDTO.StatusCode = StatusCodes.Status400BadRequest;
                responseDataDTO.message = ex.Message;
                return responseDataDTO;
            }

        }
        #endregion


        #region Get All Parking Booked Parking History
        [HttpGet("GetAllParkingHistory")]
        public async Task<ResponseDataDTO> GetAllParkingHistory()
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            responseDataDTO = await _bookParkingSpace.GetAllParkingHistory();
            return responseDataDTO;
        }
        #endregion

        #region Get Booked Parking History User Wise
        [HttpGet("ParkingHistoryUserWise")]
        public async Task<ResponseDataDTO> ParkingHistoryUserWise()
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var loginUser = User.Identity.Name;
            if (loginUser != null)
            {
                responseDataDTO.data = await _bookParkingSpace.GetParkingHistoryUserWise(loginUser);
                //var json = new JsonResult(myData, new JsonSerializerOptions
                //{
                //    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                //});
                //responseDataDTO.data = json;
                return responseDataDTO;

            }

            responseDataDTO.StatusCode = StatusCodes.Status404NotFound;
            responseDataDTO.message = "User Not Found";
            return responseDataDTO;

        }

        #endregion

        #region GetAllParkingSpace BY Using Stored Procedure
        //[HttpGet]
        //[Route("GetAllParkingSpace")]
        //public async Task<ResponseDataDTO> GetAllParkingSpace()
        //{
        //    SqlParameter[] paraObjects = new SqlParameter[50];
        //    var result = await _bookParkingSpace.spservice(paraObjects);
        //    responseDataDTO.data = result;
        //    return responseDataDTO;
        //}
        #endregion


        [HttpGet("TestAPI")]
        public async Task<ResponseDataDTO> TestAPI()
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            string wwwPath = _Environment.WebRootPath + "\\Images\\" + "aftercrop.png";
            string contentPath = _Environment.ContentRootPath;

            var addresses = _server.Features.Get<IServerAddressesFeature>().Addresses;
            var imagepath = ((string[])addresses)[1] + "Images\\" + "aftercrop.png";
            responseDataDTO.data = imagepath;
            return responseDataDTO;
        }
    }
}
