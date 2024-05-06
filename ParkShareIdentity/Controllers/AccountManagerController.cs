using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ParkShareIdentity.Data;
using ParkShareIdentity.Model;
using ParkShareIdentity.Service.Interface;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.ExternalLoginModel;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Shared.Interface;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.Swagger.Annotations;
using ParkShareIdentity.Shared;
using ParkShareIdentity.Response;

namespace ParkShareIdentity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountManagerController : ControllerBase
    {
        #region dependency Injencution
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration _configuration;
        ResponseDataDTO responseDataDTO = new ResponseDataDTO();
        ValidateModelState validateModelState = new ValidateModelState();

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        private readonly IEmailSender _emailSender;
        private readonly IRegisterService _registerService;
        private readonly ISwaggerResponse _swaggerResponse;
        public DynamicResponse dr = new DynamicResponse();
        public AccountManagerController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, IEmailSender emailSender,
             IRegisterService registerService,
              ISwaggerResponse swaggerResponse)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _registerService = registerService;
            _swaggerResponse = swaggerResponse;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        [Route("Registration")]
        public async Task<ResponseDataDTO> Register(RegisterModel userData)
        {
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = 400;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property value";
                return responseDataDTO;                
            }            
            var result = await _registerService.RegisterUser(userData);
            responseDataDTO.StatusCode = result.StatusCode;

            responseDataDTO.data=result.data;

            responseDataDTO.message = result.message;
           // return dr.Ok(result.StatusCode,result.message);
            return responseDataDTO;
            
        }
        [HttpPost("Login2")]
        public IActionResult LoginUser(LoginDTO loginModel)
        {
            return Ok("success");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ResponseDataDTO> Login(LoginDTO _userData)
        {
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = 400;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property value";
                return responseDataDTO;
            }
            
            var result = await _registerService.LoginService(_userData);
            responseDataDTO.StatusCode = result.StatusCode;
            responseDataDTO.data = result.data;
            responseDataDTO.message = result.message;
            return responseDataDTO;
        }
       // [Authorize]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<ResponseDataDTO> ResetPassword(ResetPasswordDTO _userData)
        {
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = 400;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property value";
                return responseDataDTO;
            }

            var result = await _registerService.ResetPassword(_userData);
            responseDataDTO.StatusCode = result.StatusCode;
            responseDataDTO.data = result.data;
            responseDataDTO.message = result.message;
            return responseDataDTO;
        }
       

        //Get User profile by their email id.
        //[Authorize]
        [HttpGet]
        [Route("GetUserProfile")]

        public async Task<ResponseDataDTO> GetUserProfile()
        {
            var a = User.Identity.Name;            
            var result = await _registerService.GetUserProfile(a);
            responseDataDTO.StatusCode = result.StatusCode;
            responseDataDTO.data = result.data;
            responseDataDTO.message = result.message;
            return responseDataDTO;
        }

        [HttpPost]
        [Route("ChangeAddress")]
        public async Task<ResponseDataDTO> ChangeAddress(AddressDTO addressData)
        {
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = 400;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property value";
                return responseDataDTO;
            }
            var userName = User.Identity.Name;
            
            var result = await _registerService.ChangeAddress(addressData,userName);
            responseDataDTO.StatusCode = result.StatusCode;
            responseDataDTO.data = result.data;
            responseDataDTO.message = result.message;
            return responseDataDTO;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ConfirmationEmail")]
        public async Task<ResponseDataDTO> ConfirmationEmail(ConfirmationEmailDTO _userData)
        {
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = 400;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property value";
                return responseDataDTO;
            }
            var result = await _registerService.ConfirmationEmail(_userData);
            responseDataDTO.StatusCode = result.StatusCode;
            responseDataDTO.data = result.data;
            responseDataDTO.message = result.message;
            return responseDataDTO;
        }

        //[Authorize]
        //Send Reset Password Link To Registered Email
        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<ResponseDataDTO> ForgotPassword(ForgotPasswordDTO _userData)
        {
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = 400;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property value";
                return responseDataDTO;
            }
            
            var result = await _registerService.ForgotPassword(_userData);
            responseDataDTO.StatusCode = result.StatusCode;
            responseDataDTO.data = result.data;
            responseDataDTO.message = result.message;
            return responseDataDTO;
        }
        //Change Password when User logged in.
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ResponseDataDTO> ChangePassword(ChangePasswordDTO _userData)
        {
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = 400;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property value";
                return responseDataDTO;
            }
            var userName = User.Identity.Name;
            
            var result = await _registerService.ChangePassword(_userData, userName);
            responseDataDTO.StatusCode = result.StatusCode;
            responseDataDTO.data = result.data;
            responseDataDTO.message = result.message;
            return responseDataDTO;

        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<ResponseDataDTO> DeleteUser(string UserName)
        {
            var userName = User.Identity.Name;
            
            var result = await _registerService.DeleteUser(UserName);
            responseDataDTO.StatusCode = result.StatusCode;
            responseDataDTO.data = result.data;
            responseDataDTO.message = result.message;
            return responseDataDTO;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefereshToken")]
        public async Task<ResponseDataDTO> RefereshToken(RefreshTokenDTO refreshTokenDTO)
        {
            var lstFieldError = validateModelState.CheckModelStateIsValid(ModelState);
            if (lstFieldError != null)
            {
                responseDataDTO.StatusCode = StatusCodes.Status400BadRequest;
                responseDataDTO.data = lstFieldError;
                responseDataDTO.message = "Invalid property value";
                return responseDataDTO;
            }

            if (refreshTokenDTO.accesToken != null && refreshTokenDTO.refreshToken != null)
            {
                var principal = await  _registerService.RefreshToken(refreshTokenDTO);
                    //responseDataDTO.StatusCode = principal.StatusCode;
                    //responseDataDTO.data = principal.data;
                    return principal;
                
                //else
                //{
                //    responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                //    responseDataDTO.message = "Access token is not valid";
                //    return responseDataDTO;
                //}
            }
            else
            {
                responseDataDTO.StatusCode = StatusCodes.Status400BadRequest;
                responseDataDTO.message = "Access Token Can't be Empty.";
                return responseDataDTO;
            }


        }

    }
}
