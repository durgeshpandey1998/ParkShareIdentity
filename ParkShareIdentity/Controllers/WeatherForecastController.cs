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
using System.Net;
using System.Xml.Linq;

namespace ParkShareIdentity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        #region Dependency Injencution
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration _configuration;
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        private readonly IEmailSender _emailSender;
        private readonly IRegisterService _registerService;
        private readonly ISwaggerResponse _swaggerResponse;

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IConfiguration configuration, IEmailSender emailSender,
             IRegisterService registerService,
              ISwaggerResponse swaggerResponse)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _registerService = registerService;
            _swaggerResponse = swaggerResponse;
        }
        #endregion

        [HttpPost]
        [Route("TestAPI")]
        public void Test(string Name)
        {

        }


        #region AllLogin And Register Code
        /*

        [HttpPost]
       [Route("Registeration")]
        public async Task<IActionResult> Registeration(RegisterModel _userData)
        {
            string returnUrl = null;
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var getdata = _userManager.FindByEmailAsync(_userData.Email);
            if (getdata == null)
            {

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = _userData.Email,
                        Email = _userData.Email,
                        City = _userData.City,
                        FirstName = _userData.FirstName,
                        ZIPCode = _userData.ZIPCode,
                        Street = _userData.Street,
                        latitude = _userData.latitude,
                        longitude = _userData.longitude
                    };
                    var result = await _userManager.CreateAsync(user, _userData.Password);
                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var username = _userData.Email;
                        var password = _userData.Password;
                        #region Send Mail
                        var test = "https://localhost:44374/Identity/Account/ConfirmEmail?userId=" + user.Id + "&code=" + code + "&returnUrl=" + returnUrl;
                        await _emailSender.SendEmailAsync(_userData.Email, "Confirm your email",
                                                        $"<h3>Congratulation ! You are registered.</h3><br>" +
                                                        $"<p>Thank you for showing your interest in my website.</p>" +
                                                        $"Please confirm your account by Clicking Button Below. <br> <p>_____________________________________________</p><br><a style='display: block; width: 115px; height: 25px;  background: #4E9CAF; padding: 10px;text-align: center; border-radius: 5px;color: white; font-weight: bold;line-height: 25px;' href='{HtmlEncoder.Default.Encode(test)}'>Registration</a><br>" +
                                                        $"<h3>Please Keep Your Credentials Confedential for future use.</h3><br>" +
                                                        $"<p>UserName={HtmlEncoder.Default.Encode(username)} <br> Password={HtmlEncoder.Default.Encode(password)}</p>.");
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            //await _emailSender.SendEmailAsync(Input.Email, "Registation Successfull", $"<h2>Thank You For Registration Successfully.</h2>");
                            //TempData["RegistrationSuccess"] = "Registration Successfull";
                            // return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                            // return RedirectToAction("Login","Identity");
                        }
                        #endregion
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                    }
                    else
                    {
                        return StatusCode(500, "Field can't be empty");
                    }
                    foreach (var error in result.Errors)
                    { 
                      //  ModelState.AddModelError(string.Empty, error.Description);
                        return StatusCode((Convert.ToInt32(error.Code)), "Field can't be empty");
                    }
                }

            }
            else
            {
                return StatusCode(200, "User Already Exist.");
            }
            return StatusCode(200,"Successfully Registered Now you can login");
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult>Login(LoginDTO _userData)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(_userData.Email);
                var result = await _signInManager.PasswordSignInAsync(_userData.Email, _userData.Password, _userData.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                   var Token= GenerateJsonWebToken(_userData);
                    return StatusCode(200,Token);
                }       
                if (result.IsLockedOut)
                {
                    //  var forgotPassLink = Url.Action(nameof(ForgotPassword), "Account", new { }, Request.Scheme);
                    //var content = string.Format("Your account is locked out, to reset your password, please click this link: {0}", forgotPassLink);
                    // var message = new Message(new string[] { userModel.Email }, "Locked out account information", content, null);
                    // await _registerService.EmailSend(_userData);
                    ModelState.AddModelError("", "The account is locked out");
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Ok();
                }
            }   
            return Ok();
        }

        [HttpPost]
        [Route("ConfirmationEmail")]
        public async Task<IActionResult> ConfirmationEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
          //  StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Ok();
        }


        //Send Reset Password Link To Registered Email
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO _userData)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(_userData.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                string senttime = DateTime.Now.ToString();
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", senttime, code },
                    protocol: Request.Scheme);
#pragma warning disable CS8604 // Possible null reference argument.
                await _emailSender.SendEmailAsync(
                    _userData.Email,
                    "Reset Password",
                      $"<h3>Do'nt worry ! You can reset your password.</h3><br>" +
                        $"<p>Thank you for showing your interest in my website.</p>" +
                        $"Please reset your password by Clicking Button Below. <br> <p>_____________________________________________</p><br><a style='display: block; width: 115px; height: 25px;  background: #4E9CAF; padding: 10px;text-align: center; border-radius: 5px;color: white; font-weight: bold;line-height: 25px;' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Reset Password</a>.");
#pragma warning restore CS8604 // Possible null reference argument.

                //TempData["CheckYourEmail"] = "Check Your Email To Reset Your Password";
                //  ViewData["CheckYourEmail"] = "Check Your Email to reset your password.";
                return Ok();
            }
            return Ok();
        }

        //After Clicking the forgot password link which was get in their mail

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult>ResetPassword(ResetPasswordDTO _userData)
        {
            if (!ModelState.IsValid)
            {
                return Ok();
            }

            var user = await _userManager.FindByEmailAsync(_userData.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }
            //code will be which was sent to the user's email at the time of forgot password
            var result = await _userManager.ResetPasswordAsync(user, _userData.Code, _userData.Password);
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(_userData.Email, "Password Changed Succesfully",
                       $"<h3>Congratulation ! Your password has been changed successfully..</h3><br>" +
                       $"<h3>Please Keep Your Credentials Confedential for future use.</h3><br>" +
                       $"<p>Username={user} <br> Password={_userData.Password}</p>.");


                return RedirectToPage("./Login");
            }

            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
            return Ok();
        }


        //Get User profile by their email id.
        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
           // var userdata = _userManager.GetUserName(User);
            var getdata = _userManager.FindByEmailAsync("dpd@narola.email");
           // var userProfile = _userManager.FindByIdAsync(userdata);
            return Ok(getdata);
        }

        //Change Address by the logged in UserName.
        [HttpPost]
        [Route("ChangeAddress")]
        public async Task<IActionResult> ChangeAddress(AddressDTO addressData)
        {
           // var getdata = _userManager.FindByEmailAsync("dpd@narola.email");
           // var gedata2 = _userManager.FindByIdAsync("6b644e2e-def9-41ea-8b4d-c40c19df2543");
            ApplicationUser user = new ApplicationUser();
           user = await _userManager.FindByEmailAsync(addressData.UserName);
            user.UserName = addressData.UserName;
            user.longitude = addressData.longitude;
            user.latitude = addressData.latitude;
            user.City = addressData.City;
            user.ZIPCode = addressData.ZIPCode;
            user.Street = addressData.Street;
             var data = await _userManager.UpdateAsync(user);
            return StatusCode(200,addressData);
        }
        //Change Password when User logged in.
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO _userData)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, _userData.OldPassword, _userData.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Ok();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
           var  StatusMessage = "Your password has been changed.";
            return Ok(StatusMessage);
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string UserName)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = UserName;
            var data = await _userManager.DeleteAsync(user);
            return StatusCode(200,data);
        }

        #region Generate Token Action Method
         [HttpPost]
         [Route("Post")]
         public async Task<IActionResult> Post(LoginDTO _userData)
         {
             var user = await _userManager.FindByNameAsync(_userData.Email);
             if (user != null && await _userManager.CheckPasswordAsync(user, _userData.Password))
             {
                // var userRoles = await _userManager.GetRolesAsync(user);

                 //var authClaims = new List<Claim>
                 //{
                 //    new Claim(ClaimTypes.Name, user.UserName),
                 //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 //};
                 var authClaims = new List<Claim>
                 {
                     new Claim(ClaimTypes.Name, _userData.Email),
                      new Claim(ClaimTypes.GivenName, _userData.Email),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 };
                 //foreach (var userRole in userRoles)
                 //{
                 //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                 //}

                 var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey"));

                 var token = new JwtSecurityToken(
                     issuer: _configuration["JWT:ValidIssuer"],
                     audience: _configuration["JWT:ValidAudience"],
                     expires: DateTime.Now.AddHours(3),
                     claims: authClaims,
                     signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                     );
                 return Ok(new
                 {
                     token = new JwtSecurityTokenHandler().WriteToken(token),
                     expiration = token.ValidTo
                 });
             }
             return Unauthorized();

         }
        
        #endregion

        //Generate Token For Login..
        private string GenerateJsonWebToken(LoginDTO _userData)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _userData.Email),
                     new Claim(ClaimTypes.GivenName, _userData.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey"));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        */
        #endregion



        //[HttpGet]
        //[Route("GetAPIKey")]
        //public IActionResult GetAPIKey()
        //{
        //    string address = "123 something st, somewhere";
        //    string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", Uri.EscapeDataString(address),"");

        //    WebRequest request = WebRequest.Create(requestUri);
        //    WebResponse response = request.GetResponse();
        //    XDocument xdoc = XDocument.Load(response.GetResponseStream());

        //    XElement result = xdoc.Element("GeocodeResponse").Element("result");
        //    XElement locationElement = result.Element("geometry").Element("location");
        //    XElement lat = locationElement.Element("lat");
        //    XElement lng = locationElement.Element("lng");
        //    return Ok();
        //}




    }
}