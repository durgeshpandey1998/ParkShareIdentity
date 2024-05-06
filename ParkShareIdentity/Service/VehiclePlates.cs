using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ParkShareIdentity.Areas.Identity.Data;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Service.Interface;
using System.Text.RegularExpressions;

namespace ParkShareIdentity.Service
{
    #region Dependency
    public class VehiclePlates : IVehiclePlates
    {

        public IConfiguration _configuration;

        //   public IList<AuthenticationScheme> ExternalLogins { get; set; }
        private readonly IEmailSender _emailSender;
        private readonly IRegisterService _registerService;
        private readonly ParkShareIdentityContext _parkShareIdentityContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public VehiclePlates(

            IConfiguration configuration,
            IEmailSender emailSender,
             IRegisterService registerService,
             ParkShareIdentityContext parkShareIdentityContext,
             UserManager<ApplicationUser> userManager)
        {

            _configuration = configuration;
            _emailSender = emailSender;
            _registerService = registerService;
            _parkShareIdentityContext = parkShareIdentityContext;
            _userManager = userManager;

        }
        #endregion
        public async Task<ResponseDataDTO> AddVehiclePlate(VehicleDTO vehicleDTO,string UserName)
        {
            try
            {
                var getUserId = await _userManager.FindByEmailAsync(UserName);
                ResponseDataDTO responseData = new ResponseDataDTO(); 
                var isNumberPlateCheck = _parkShareIdentityContext.Vehicles.SingleOrDefault(x => x.VehicleNumber == vehicleDTO.NumberPlate);
               
                Regex r = new Regex(@"^[a-zA-Z][a-zA-Z][0-9]{5}$");
                Vehicle data = new Vehicle();
                // string regexPlateSubmit = "^[a-zA-Z][a-zA-Z][0-9]+$";
                if (r.IsMatch(vehicleDTO.NumberPlate))
                {
                    data.VehicleNumber = vehicleDTO.NumberPlate;
                    data.UserId = getUserId.Id;
                    data.VehicleType = vehicleDTO.VehicleType;
                    //_parkShareIdentityContext.Vehicles.Add(data);
                    if (isNumberPlateCheck==null)
                    {
                        _parkShareIdentityContext.Vehicles.Add(data);
                        _parkShareIdentityContext.SaveChanges();
                        responseData.StatusCode = StatusCodes.Status200OK;
                        responseData.data = "Vehicle addedd Successfuly.";
                        return responseData;
                    }
                    else
                    {
                        responseData.StatusCode = StatusCodes.Status200OK;
                        responseData.data = "Vehicle Already Added.";
                        return responseData;
                    }
                   
                }
                else
                {
                    responseData.StatusCode = StatusCodes.Status406NotAcceptable;
                    responseData.message = "Number Plate Pattern Not Match.";
                    return responseData;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ResponseDataDTO> UpdateVehiclePlates(VehicleUpdateDTO vehicleDTO)
        {
            ResponseDataDTO responseData = new ResponseDataDTO();
            try
            {
                Regex r = new Regex(@"^[a-zA-Z][a-zA-Z][0-9]{5}$"); //Number Plate Example: GJ05234  
                Vehicle data = new Vehicle();
               // var isIdCheck = _parkShareIdentityContext.Vehicles.SingleOrDefault(x => x.VehicleId == vehicleDTO.VehicleId);
                var isIdCheck = _parkShareIdentityContext.Vehicles.SingleOrDefault(x => x.VehicleNumber == vehicleDTO.NumberPlate);
                if (vehicleDTO.NumberPlate != null)
                {
                    if (isIdCheck == null)
                    {
                        data = _parkShareIdentityContext.Vehicles.FirstOrDefault(x => x.VehicleId == vehicleDTO.VehicleId);
                        if (data!=null)
                        {
                            data.VehicleNumber = vehicleDTO.NumberPlate;
                            data.VehicleType = vehicleDTO.VehicleType;
                            _parkShareIdentityContext.Vehicles.Update(data);
                            _parkShareIdentityContext.SaveChanges();

                            responseData.StatusCode = StatusCodes.Status200OK;
                            responseData.message = "Vehicle update Successfully.";
                            return responseData;
                        }
                        else
                        {
                            responseData.StatusCode = StatusCodes.Status404NotFound;
                            responseData.message = "No data fund with given id.";
                            return responseData;
                        }

                    }
                    else
                    {
                        responseData.StatusCode = StatusCodes.Status404NotFound;
                        responseData.message = "This Vehicle Already Added.";
                        return responseData;
                    }

                }
                else
                {
                    responseData.StatusCode = StatusCodes.Status406NotAcceptable;
                    responseData.message = "Number Plate Pattern Not Match.";
                    return responseData;
                }
            }
            catch (Exception ex)
            {

                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.message = "Internal Server Error.";
                return responseData;
            }
        }

        public async Task<ResponseDataDTO> DeleteVehicles(int Vid)
        {
            ResponseDataDTO responseData = new ResponseDataDTO();
            try
            {
                var deleteQuery = _parkShareIdentityContext.Vehicles.Find(Vid);
                if (deleteQuery != null)
                {
                    _parkShareIdentityContext.Entry(deleteQuery).State = EntityState.Deleted;
                    _parkShareIdentityContext.SaveChanges();
                    responseData.StatusCode = StatusCodes.Status200OK;
                    responseData.data = "Vehicle Deleted Successfuly.";
                    return responseData;
                }
                else
                {
                    responseData.StatusCode = StatusCodes.Status404NotFound;
                    responseData.data = "Given Vehicle is Not Found.";
                    return responseData;
                }
            }
            catch (Exception e)
            {
                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.data = "Internal Server Error.";
                return responseData;
            }
        }

        public async Task<ResponseDataDTO> GetVehicleById(int vehicleId)
        {
            ResponseDataDTO responseData = new ResponseDataDTO();
            var data = await _parkShareIdentityContext.Vehicles.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
            if (data == null)
            {
                responseData.StatusCode = StatusCodes.Status404NotFound;
                responseData.data = "Vehicle Not Found.";
                return responseData;
            }
            else
            {
                var result =  _parkShareIdentityContext.Vehicles.Find(vehicleId);
                var user = await _userManager.FindByIdAsync(result.UserId);
               result.UserId = user.UserName;
                VehicleUserDto vehicledata = new VehicleUserDto();
                vehicledata.UserName = user.UserName;
                vehicledata.VehicleType = result.VehicleType;
                vehicledata.VehicleId = result.VehicleId;
                vehicledata.licencensePlateNumber = result.VehicleNumber;
                responseData.StatusCode = StatusCodes.Status200OK;
                responseData.data = vehicledata;
                return responseData;
            }

        }

        public async Task<ResponseDataDTO> GetVehicleByUserId(string UserId)
        {
            //var result = await _parkShareIdentityContext.Vehicles.FindAsync(UserId);

            ResponseDataDTO responseData = new ResponseDataDTO();

            try
            {
                if (UserId!=null)
                {
                    List<Vehicle> result = _parkShareIdentityContext.Vehicles.ToList();
                    List<string> data = new List<string>();
                    List<VehicleUserDto> vehicleData = new List<VehicleUserDto>();

                    for (int i = 0; i < result.Count; i++)
                    {
                        var user = await _userManager.FindByIdAsync(result[i].UserId);

                        VehicleUserDto data1 = new VehicleUserDto();
                        data1.VehicleId = result[i].VehicleId;
                        data1.VehicleType = result[i].VehicleType;
                        data1.licencensePlateNumber = result[i].VehicleNumber;
                        data1.UserName = user.UserName;
                        if (result[i].applicationuser.UserName == UserId)
                        {
                            vehicleData.Add(data1);
                        }

                    }
                 
                    if (vehicleData.Count>0)
                    {
                        responseData.StatusCode = StatusCodes.Status200OK;
                        responseData.data = vehicleData;
                    }
                    else
                    {
                        responseData.StatusCode = StatusCodes.Status404NotFound;
                        responseData.message = "Invalid User Id.";
                    }
                   
                    return responseData;
                }
                else
                {
                    responseData.StatusCode = StatusCodes.Status404NotFound;
                    responseData.data = "Given UserId is Not Found.";
                    return responseData;
                }
            }
            catch (Exception ex)
            {


                responseData.StatusCode = StatusCodes.Status500InternalServerError;
                responseData.data = "Internal Server Error.";
                return responseData;
            }
        }

        public async Task<ResponseDataDTO> GetAllVehicle()
        {
            ResponseDataDTO responseData = new ResponseDataDTO();

            List<Vehicle> result = _parkShareIdentityContext.Vehicles.ToList();
            List<string> data = new List<string>();
            List<VehicleUserDto> vehicleData = new List<VehicleUserDto>();
            var a = result[0].UserId;
            for (int i = 0; i < result.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(result[i].UserId);
                data.Add(user.UserName);
                VehicleUserDto data1 = new VehicleUserDto();
                data1.VehicleId = result[i].VehicleId;
                data1.VehicleType = result[i].VehicleType;
                data1.licencensePlateNumber = result[i].VehicleNumber;
                data1.UserName = user.UserName;
                vehicleData.Add(data1);
            }
            if (vehicleData.Count > 0)
            {
                responseData.StatusCode = StatusCodes.Status200OK;
                responseData.data = vehicleData;
            }
            else
            {
                responseData.StatusCode = StatusCodes.Status404NotFound;
                responseData.message = "Invalid User Id.";
            }
            return responseData;


        }

        private bool UploadFile(IFormFile ImageData)
        {
            string fileNameWithPath = "";
            string path = "D:\\ParkShareIdentity\\ParkShareIdentity\\ParkShareIdentity\\Service\\Images\\";
            //string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Demo1.Web.Host/wwwroot/Documents2");
            fileNameWithPath = Path.Combine(path, ImageData.FileName); //+ DateTime.Now.ToString("yyyyMMddHHmmss");
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                ImageData.CopyTo(stream);
                return true;
            }
        }



    }
}
