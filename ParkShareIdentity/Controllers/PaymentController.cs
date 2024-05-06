using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Response;
using ParkShareIdentity.Service.Interface;
using ParkShareIdentity.Shared;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Swashbuckle.AspNetCore.Annotations;

namespace ParkShareIdentity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
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
        private readonly IPayment _payment;
        public PaymentController(IAddSpace addSpace, IMasterAddress masterAddress, IConfiguration configuration,
          UserManager<ApplicationUser> userManager,
          ParkShareIdentityContext ParkShareIdentityContext, IBookParkingSpace bookParkingSpace, IServer server,
          IHostingEnvironment Environment, IPayment payment)
        {
            _addSpace = addSpace;
            _masterAddress = masterAddress;
            _configuration = configuration;
            _userManager = userManager;
            _ParkShareIdentityContext = ParkShareIdentityContext;
            _bookParkingSpace = bookParkingSpace;
            _server = server;
            _Environment = Environment;
            _payment = payment;
        }
        #endregion

        [HttpPost("Recharge")]
        [SwaggerOperation(Summary = "User Can Recharge his wallet.")]
        public async Task<ResponseDataDTO> Recharge([FromBody] RechargeDTO rechargeDTO)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            if (rechargeDTO != null && rechargeDTO.Amount > 0)
            {
                var principal = User.Identity.Name;
                responseDataDTO.data = await _payment.Recharge(rechargeDTO, principal);
                return responseDataDTO;
            }
            responseDataDTO.message = "Amount Should be greater than 0";
            responseDataDTO.StatusCode = StatusCodes.Status200OK;
            return responseDataDTO;
        }
        [HttpPost("Payment")]
        [SwaggerOperation(Summary = "User can make payment for space booking.")]
        public async Task<ResponseDataDTO> Payment(int BookingId)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            if (BookingId>0)
            {
                responseDataDTO.data = await  _payment.PaymentBooking(BookingId);
                //responseDataDTO.StatusCode = StatusCodes.Status200OK;
                //responseDataDTO.message = "Payment Completed Successfully";
                return responseDataDTO;
            }
            else
            {
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                responseDataDTO.message = "Booking Id Should be greater than 0.";
                return responseDataDTO;
            }
         
        }

        [HttpGet("GetUserBalance")]
        [SwaggerOperation(Summary = "User can get his wallet Details.")]
        public async Task<ResponseDataDTO> GetUserBalance()
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            try
            {
                var Principal = User.Identity.Name;
                responseDataDTO = await _payment.GetWalletBalance(Principal);
                return responseDataDTO;
            }
            catch (Exception ex)
            {
                responseDataDTO.message = ex.Message;
                responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                return responseDataDTO;
            }
        }

        [HttpGet("CancelBooking")]
        [SwaggerOperation(Summary ="User can cancel his booking")]
        public async Task<ResponseDataDTO> CancelBooking(int BookingId)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var principal = User.Identity.Name;
            if (BookingId>0 && principal!=null)
            {
                responseDataDTO.data = await  _payment.CancelBooking(BookingId,principal);
                return responseDataDTO;
            }
               else
            {
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                responseDataDTO.message = "Booking Id Should be greater than 0. Or You are not logged in.";
                return responseDataDTO;
            }
        }

        [HttpGet("TransactionHistory")]
        [SwaggerOperation(Summary ="User can get his transaction history.")]
        public async Task<ResponseDataDTO> TransactionHistory()
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var principal = User.Identity.Name;
            responseDataDTO = await _payment.TransactionHistory(principal);
            return responseDataDTO;
        }
    }
}
