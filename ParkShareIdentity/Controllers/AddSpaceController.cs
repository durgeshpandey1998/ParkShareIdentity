using ClosedXML.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Enums;
using ParkShareIdentity.Response;
using ParkShareIdentity.Service;
using ParkShareIdentity.Service.Interface;
using ParkShareIdentity.Shared;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.IO;

namespace ParkShareIdentity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddSpaceController : ControllerBase
    {
        public string str = "";

        private readonly IAddSpace _addSpace;
        private readonly IMasterAddress _masterAddress;
        private MasterAddressDTO masterAddressViewModel = new MasterAddressDTO();
        public IConfiguration _configuration;
        private UserManager<ApplicationUser> _userManager;
        DynamicResponse dr = new DynamicResponse();
        ValidateModelState validateModelState = new ValidateModelState();
        private readonly IWebHostEnvironment webHostEnvironment;
        public object XLEventTracking { get; private set; }

        public AddSpaceController(IAddSpace addSpace, IMasterAddress masterAddress, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _addSpace = addSpace;
            _masterAddress = masterAddress;
            _configuration = configuration;
            _userManager = userManager;
            webHostEnvironment = hostEnvironment;

        }

        [HttpPost("SaveImages")]
        public async Task<IActionResult> SaveImages([FromForm] ImagesViewModelDTO data)
        {
            var principal = User.Identity.Name;
            var currentUserData = await _userManager.FindByNameAsync(principal);
            data.UserId = currentUserData.Id;
            return _addSpace.ValidateImagesViewModel(data);
        }

        [HttpPost("AddMasterAddress")]
        public async Task<IActionResult> AddMasterAddress([FromBody] MasterAddressDTO data)
        {
            var principal = User.Identity.Name;
            var currentUserData = _userManager.FindByNameAsync(principal);
            data.UserId = currentUserData.Result.Id;

            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                return dr.Ok(404, "Bad request", lstFieldError);
            }

            return _masterAddress.AddMasterAddress(data);

        }


        [HttpPost("AddSpace12")]
        public async Task<IActionResult> AddSpace12([FromBody] AddSpaceDTO data)
        {
            //Get current user data and set userId
            if (data == null)
            {
                return dr.Ok("Please enter proper data");
            }
            #region Get error list from ModelState if any
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                return dr.Ok(404, "Bad request", lstFieldError);
            }
            #endregion
            var principal = User.Identity.Name;
            if (principal == null)
            {
                return dr.Ok("You are not logged in");
            }
            var currentUserData = await _userManager.FindByNameAsync(principal);
            var value = currentUserData.Id;
            if (value != null)
            {
                data.UserId = value;
            }
            else
            {
                return dr.Ok("Current login User Id Not Found");
            }
            foreach (var item in data.DateTime1)
            {
                if (item.From < DateTime.Now || item.To < DateTime.UtcNow || item.From == item.To)
                {
                    return dr.Ok("Please select date and time greater than Current Date Time.");
                }
            }
            return _addSpace.ValidateAddSpaceViewModel(data);
        }

        [HttpPost("AddSpace")]
        public async Task<ResponseDataDTO> AddSpace([FromBody] AddSpaceDTO data)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var principal = User.Identity.Name;
            if (principal == null)
            {
                responseDataDTO.message = "You are not login";
            }
            #region Get error list from ModelState if any
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                return dr.Ok(404, "Bad request", lstFieldError);
            }
            #endregion
            var currentUserData = await _userManager.FindByNameAsync(principal);
            var value = currentUserData.Id;
            if (value != null)
            {
                data.UserId = value;
            }
            else
            {
                responseDataDTO.message = "Current login User Id Not Found";
                return responseDataDTO;
            }
            if (data.DateTime1 != null)
            {
                foreach (var item in data.DateTime1)
                {
                    if (item.From < DateTime.Now || item.To < DateTime.UtcNow || item.From == item.To)
                    {
                        responseDataDTO.message = "Please select date and time greater than Current Date Time.";
                        return responseDataDTO;
                    }
                }
            }
            responseDataDTO.data = await _addSpace.AddSpaceData(data, principal);
            return responseDataDTO;
        }

        [HttpGet("GetMasterAddressList")]
        public async Task<ResponseDataDTO> GetMasterAddressList()
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var principal = User.Identity.Name;
            var data = await _masterAddress.GetMasterAddressList(principal);
            responseDataDTO.data = data;
            return responseDataDTO;

        }

        [HttpGet("EditMasterAddress")]
        public async Task<ResponseDataDTO> EditMasterAddress(int masterAddressId)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            responseDataDTO.data = await _masterAddress.EditMasterAddress(masterAddressId);
            return responseDataDTO;
        }

        [HttpPut("UpdateMasterAddress")]
        public async Task<ResponseDataDTO> UpdateMasterAddress([FromBody] MasterAddressUpdateDTO data)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            #region Get error list from ModelState if any
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = StatusCodes.Status404NotFound;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property.";
                return responseDataDTO;
            }
            #endregion
            var principal = User.Identity.Name;
            var currentUserData = await _userManager.FindByNameAsync(principal);
            data.UserId = currentUserData.Id;
            return await _masterAddress.UpdateMasterAddress(data);
        }

        [HttpDelete("DeleteSpace")]
        public async Task<ResponseDataDTO> DeleteSpace(int SpaceId)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var principal = User.Identity.Name;
            if (SpaceId > 0)
            {
                return responseDataDTO = await _addSpace.DeleteSpace(SpaceId, principal);
                //responseDataDTO.StatusCode = StatusCodes.Status200OK;
                //responseDataDTO.message = "Space deleted successfully";
            }
            responseDataDTO.StatusCode = StatusCodes.Status200OK;
            responseDataDTO.message = "This space is not available.";
            responseDataDTO.data = "null";
            return responseDataDTO;

        }

        [HttpPut("DownloadFile")]
        public ActionResult DownloadFile()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
          new DataColumn("Name", typeof(string)),
          new DataColumn("Country",typeof(string)) });
            dt.Rows.Add(1, "C Sharp corner", "United States");
            dt.Rows.Add(2, "Suraj", "India");
            dt.Rows.Add(3, "Test User", "France");
            dt.Rows.Add(4, "Developer", "Russia");
            using (var workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(dt, "Customers");

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                           "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "XXXName.xlsx"
                    };
                }
            }

        }


    }
}
