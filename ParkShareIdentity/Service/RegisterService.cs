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
using ParkShareIdentity.DTO;
using ParkShareIdentity.Shared.Interface;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Policy;
using System.Net;
using System.Net.Mail;
using System.Web.Http.ModelBinding;
using Azure.Core;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Logging;

namespace ParkShareIdentity.Service
{
    public class RegisterService : ControllerBase,IRegisterService
    {
        #region List of Dependency
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        private readonly SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public readonly IConfiguration _configuration;
        private readonly ParkShareIdentityContext _ParkShareIdentityContext;
        public RegisterService()
        {

        }
        public RegisterService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration,
            ParkShareIdentityContext ParkShareIdentityContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _ParkShareIdentityContext = ParkShareIdentityContext;
        }
        #endregion

        #region Email Send Code
        public string EmailSend(RegisterModel userData)
        {

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("abpnarola@gmail.com", "uljshtzeaxczdcsw");

            String from = "abpnarola@gmail.com";
            String to = userData.Email;
            String subject = "Registration Successfully.";
            //string path = HttpContext.Current.Server.MapPath("~/files/sample.html");
            string resultPath = "E:\\ParkShareFolder\\ParkShare\\ParkShare\\Controllers\\EmailTemplate.html";
            string content = System.IO.File.ReadAllText(resultPath);
            String messageBody = content;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            messageBody = messageBody.Replace("#UserName", userData.FirstName);
            messageBody = messageBody.Replace("#NewPassword", userData.Password);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            MailMessage message = new MailMessage(from, to, subject, messageBody);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
              $"{messageBody} <br> <img src=\"cid:Wedding\">",
              null,
              "text/html"
            );

            message.AlternateViews.Add(htmlView);

            try
            {
                smtp.Send(message);
            }
            catch (SmtpException ex)
            {
                return ex.Message;
            }
            return null;
        }
        #endregion

        #region Registration logic
        public async Task<ResponseDataDTO> RegisterUser(RegisterModel userData) 
        {

            try
            {
                ResponseDataDTO registerDTO = new ResponseDataDTO();
                string returnUrl = null;
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                var getUserData = await _userManager.FindByEmailAsync(userData.Email);

                if (getUserData == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userData.Email,
                        Email = userData.Email,
                        City = userData.City,
                        FirstName = userData.FirstName,
                        ZIPCode = userData.ZIPCode,
                        Street = userData.Street,
                        latitude = userData.latitude,
                        longitude = userData.longitude
                    };
                    var result = await _userManager.CreateAsync(user, userData.Password);
                   
                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var username = userData.Email;
                        var password = userData.Password;
                        #region Send Mail
                        var url = "https://localhost:44374/Identity/Account/ConfirmEmail?userId=" + user.Id + "&code=" + code + "&returnUrl=" + returnUrl;
                        await _emailSender.SendEmailAsync(userData.Email, "Confirm your email",
                                                        $"<h3>Congratulation ! You are registered.</h3><br>" +
                                                      
                                                        $"Please confirm your account by Using Below Code And UserId. <br> " +
                                                        $"<p>_______________________________________________________</p><br>" +
                                                        $"<p><b>Code=</b>{HtmlEncoder.Default.Encode(code)}<br><b>UserId=</b>{HtmlEncoder.Default.Encode(user.Id)}" +
                                                        $"<h3>Please Keep Your Credentials Confedential for future use.</h3><br>" +
                                                        $"<p>UserName={HtmlEncoder.Default.Encode(username)} <br> Password={HtmlEncoder.Default.Encode(password)}</p>.");
                        registerDTO.StatusCode = StatusCodes.Status200OK;
                        registerDTO.message = "User Created Succesfully. Please Check Your Email for Email Confirmation";
                        return registerDTO;


                        //var url = "https://localhost:44374/Identity/Account/ConfirmEmail?userId=" + user.Id + "&code=" + code + "&returnUrl=" + returnUrl;
                        //await _emailSender.SendEmailAsync(userData.Email, "Confirm your email",
                        //                                $"<h3>Congratulation ! You are registered.</h3><br>" +
                        //                                $"<p>Thank you for showing your interest in my website.</p>" +
                        //                                $"Please confirm your account by Clicking Button Below. <br> <p>_____________________________________________</p><br><a style='display: block; width: 115px; height: 25px;  background: #4E9CAF; padding: 10px;text-align: center; border-radius: 5px;color: white; font-weight: bold;line-height: 25px;' href='{HtmlEncoder.Default.Encode(url)}'>Registration</a><br>" +
                        //                                $"<h3>Please Keep Your Credentials Confedential for future use.</h3><br>" +
                        //                                $"<p>UserName={HtmlEncoder.Default.Encode(username)} <br> Password={HtmlEncoder.Default.Encode(password)}</p>.");
                        //registerDTO.StatusCode = StatusCodes.Status200OK;
                        //registerDTO.message = "User Created Succesfully. Please Check Your Email for Email Confirmation";
                        //return registerDTO;
                        #endregion
                    }
                    else
                    {
                        registerDTO.StatusCode = StatusCodes.Status400BadRequest;
                        registerDTO.message = "Bad Request.";
                        return registerDTO;
                    }
                }
                else
                {
                    registerDTO.StatusCode = StatusCodes.Status201Created;
                    registerDTO.message = "User Already Registered.";
                    return registerDTO;
                }
                
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
        #endregion
        public class LoginData
        {
            public string accessToken { get; set; }
            public string refreshToken { get; set; }
            public DateTime accessTokenValidateTo { get; set; }
            public DateTime refreshTokenValidateTo { get; set; }
        }

        #region Login  logic
        public async Task<ResponseDataDTO> LoginService(LoginDTO _userData)
        {
            ResponseDataDTO data = new ResponseDataDTO();
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(); 
            var appUser = await _userManager.FindByEmailAsync(_userData.Email);
            var result = await _signInManager.PasswordSignInAsync(_userData.Email, _userData.Password, _userData.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var Token = GenerateJsonWebToken(_userData);
                LoginData loginData = new LoginData();
                loginData.accessToken = new JwtSecurityTokenHandler().WriteToken(Token);
                loginData.refreshToken = GenerateRefreshToken();
                loginData.accessTokenValidateTo =Token.ValidTo.ToLocalTime();
                loginData.refreshTokenValidateTo= DateTime.Now.AddMinutes(30); 
                ApplicationUser userData = new ApplicationUser();
               userData = await _userManager.FindByEmailAsync(_userData.Email);
             //   userData.AccessToken = loginData.accessToken;
                userData.RefereshToken = loginData.refreshToken;
                userData.ExpiryTime = loginData.refreshTokenValidateTo;
               
                var success = await _userManager.UpdateAsync(userData);
                if (success.Succeeded)
                {
                    data.StatusCode = StatusCodes.Status200OK;
                    data.data = loginData;
                    return data;
                }
                else
                {
                    data.StatusCode = StatusCodes.Status500InternalServerError;
                    data.message = "Internal Server Error";
                    return data;
                }
            }
            if (result.IsLockedOut)
                {
                data.StatusCode = StatusCodes.Status408RequestTimeout;
                data.message = "Please try after some time.Your account locked for 10 minutes.";
                return data;
            }
            data.StatusCode = StatusCodes.Status404NotFound;
            data.message = "User Not Found.";
            return data;
        }
        #endregion

        #region Reset   Password
        public async Task<ResponseDataDTO> ResetPassword(ResetPasswordDTO _userData)
        {
            ResponseDataDTO data = new ResponseDataDTO();
            string code=null;

            var user = await _userManager.FindByEmailAsync(_userData.Email);
            if (user == null)
            {
                data.StatusCode = StatusCodes.Status302Found;
                data.message = "User Not Found.";
                return data;
            }

            if (_userData.Code!= null)
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(_userData.Code));
           
                
            }
            //code will be which was sent to the user's email at the time of forgot password
            var result = await _userManager.ResetPasswordAsync(user, code, _userData.Password);
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(_userData.Email, "Password Changed Succesfully",
                       $"<h3>Congratulation ! Your password has been changed successfully..</h3><br>" +
                       $"<h3>Please Keep Your Credentials Confedential for future use.</h3><br>" +
                       $"<p>Username={user} <br> Password={_userData.Password}</p>.");
                data.StatusCode = StatusCodes.Status200OK;
                data.message = "Password Changed Successfully.";
                return data;
            }

            data.StatusCode = StatusCodes.Status406NotAcceptable;
            data.message = " Token Expired, Please try after some Time.";
            return data;
        }
        #endregion

        
        #region GetUser Profile   
        public async Task<ResponseDataDTO> GetUserProfile(string UserName)
        {
            ResponseDataDTO data = new ResponseDataDTO();
            //var a = User.Identity.Name;
            var getdata = await _userManager.FindByEmailAsync(UserName);
            UserProfileDTO userdata = new UserProfileDTO();
            if (getdata != null)
            {
                userdata.UserProfile.FirstName = getdata.FirstName;
                userdata.UserProfile.ZIPCode = getdata.ZIPCode;
                userdata.UserProfile.City = getdata.City;
                userdata.UserProfile.Street = getdata.Street;
                userdata.UserProfile.latitude = getdata.latitude;
                userdata.UserProfile.longitude = getdata.longitude;
                userdata.UserProfile.Email = getdata.Email;
                data.StatusCode = StatusCodes.Status200OK;
                data.data = userdata;
                return data;
            }
            data.StatusCode = StatusCodes.Status406NotAcceptable;
            data.message = "Please try after some time.";
            return data;
        }
        #endregion

        #region Change   Address
        public async Task<ResponseDataDTO> ChangeAddress(AddressDTO addressData,string userName)
        {
            ResponseDataDTO data = new ResponseDataDTO();
            
            ApplicationUser user = new ApplicationUser();
            user = await _userManager.FindByEmailAsync(userName);
            if (user!=null)
            {
                user.UserName = addressData.UserName;
                user.longitude = addressData.longitude;
                user.latitude = addressData.latitude;
                user.City = addressData.City;
                user.ZIPCode = addressData.ZIPCode;
                user.Street = addressData.Street;
                var data1 = await _userManager.UpdateAsync(user);
                data.StatusCode = StatusCodes.Status200OK;
                data.data = data1;
                data.message = "Record Updated Successfully.";
                return data;
            }
            data.StatusCode = StatusCodes.Status406NotAcceptable;
            data.message = "Please try after some time.";
            return data;
        }
        #endregion

        #region Change   Password
        public async Task<ResponseDataDTO> ChangePassword(ChangePasswordDTO _userData, string userName)
        {
            ResponseDataDTO data = new ResponseDataDTO();
            
            var user = await _userManager.FindByEmailAsync(userName);
            if (user == null)
            {
                data.StatusCode = StatusCodes.Status406NotAcceptable;
                data.message = "User Not found.";
                return data;
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                data.StatusCode = StatusCodes.Status406NotAcceptable;
                data.message = "Has Password Not found.";
                return data;
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, _userData.OldPassword, _userData.NewPassword);
            if (!changePasswordResult.Succeeded)
            {

                data.StatusCode = StatusCodes.Status406NotAcceptable;
                data.message = "Please try after some time.";
                return data;
            }

            await _signInManager.RefreshSignInAsync(user);

            data.StatusCode = StatusCodes.Status200OK;
            data.message = "Your password has been changed.";
            return data;
        }
        #endregion


        #region Delete User   
        public async Task<ResponseDataDTO> DeleteUser(string userName)
        {
            ResponseDataDTO data = new ResponseDataDTO();

            ApplicationUser user = new ApplicationUser();
            user.UserName = userName;
            var data1 = await _userManager.DeleteAsync(user);
            if (data1!=null)
            {
                data.StatusCode = StatusCodes.Status200OK;
                data.message = "User deleted.";
                return data;
            }
            data.StatusCode = StatusCodes.Status406NotAcceptable;
            data.message = "Please try after some time.";
            return data;
        }
        #endregion

        #region Forgot Password  logic
           public async Task<ResponseDataDTO> ForgotPassword(ForgotPasswordDTO _userData)
           {
            try
            {
                ResponseDataDTO data = new ResponseDataDTO();
                var user = await _userManager.FindByEmailAsync(_userData.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    data.StatusCode = StatusCodes.Status302Found;
                    data.message = "User Not Found.";
                    return data;
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                string senttime = DateTime.Now.ToString();
                //var callbackUrl = Url.Page(
                //           "/Account/ResetPassword",
                //           pageHandler: null,
                //           values: new { area = "Identity", senttime, code },
                //           protocol: Request.Scheme);

                #region Email Send
                var callbackUrl = code;

                await _emailSender.SendEmailAsync(
                    _userData.Email,
                    "Reset Password",
                      $"<h3>Do'nt worry ! You can reset your password.</h3><br>" +
                        $"<p>Thank you for showing your interest in my website.</p>" +
                        $"Please reset your password by Clicking Button Below. <br> <p>_____________________________________________</p><br>" +
                        $"Code={HtmlEncoder.Default.Encode(code)}");

                #endregion




                //await _emailSender.SendEmailAsync(
                //    _userData.Email,
                //    "Reset Password",
                //      $"<h3>Do'nt worry ! You can reset your password.</h3><br>" +
                //        $"<p>Thank you for showing your interest in my website.</p>" +
                //        $"Please reset your password by Clicking Button Below. <br> <p>_____________________________________________</p><br><a style='display: block; width: 115px; height: 25px;  background: #4E9CAF; padding: 10px;text-align: center; border-radius: 5px;color: white; font-weight: bold;line-height: 25px;' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Reset Password</a>."); 




                data.StatusCode = StatusCodes.Status200OK;
                data.message = "Check Your Email To Reset Your Password";
                return data;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

           } 
        #endregion


        #region Confirmation Email
        public async Task<ResponseDataDTO> ConfirmationEmail(ConfirmationEmailDTO userConfirm)
        {
            ResponseDataDTO data = new ResponseDataDTO();

            if (userConfirm.userId == null || userConfirm.code == null)
            {
                data.StatusCode = StatusCodes.Status408RequestTimeout;
                data.message = "Session Expired Login again";
                return data;
            }
            var user = await _userManager.FindByIdAsync(userConfirm.userId);
            if (user == null)
            {
                //return NotFound($"Unable to load user with ID '{userId}'.");
                data.StatusCode = StatusCodes.Status404NotFound;
                data.message = "Unable to load user with ID " + userConfirm.userId + ".";
                return data;
            }

            userConfirm.code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userConfirm.code));
            var result = await _userManager.ConfirmEmailAsync(user, userConfirm.code);
            //  StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            if (result.Succeeded)
            {
                data.StatusCode = StatusCodes.Status200OK;
                data.message = "Email Confirmed.";
                return data;
            }
            data.StatusCode = StatusCodes.Status500InternalServerError;
            data.message = "Internal Server Error.";
            return data;
        }
        #endregion


        #region Generate Refresh Token
        public async Task<ResponseDataDTO> RefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
            ResponseDataDTO data = new ResponseDataDTO();
            LoginData loginData = new LoginData();
            if (refreshTokenDTO.accesToken != null)
            {
                var principal = GetPrincipalFromExpiredToken(refreshTokenDTO.accesToken);
               var userData = await _userManager.FindByEmailAsync(principal.Identity.Name);
                if (userData.ExpiryTime>DateTime.Now && userData.RefereshToken==refreshTokenDTO.refreshToken)
                {
                    if (principal != null)
                    {
                        var newAccessToken = GenerateJsonWebToken(principal.Claims);
                        loginData.accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                        loginData.accessTokenValidateTo = newAccessToken.ValidTo;
                        loginData.refreshTokenValidateTo = (DateTime)userData.ExpiryTime;

                        data.StatusCode = StatusCodes.Status200OK;
                        data.data = loginData;
                        return data;
                    }
                    else
                    {
                        data.StatusCode = StatusCodes.Status500InternalServerError;
                        data.message = "Internal Server Error. Unable to generate new accesstoken.";
                        return data;
                    }
                }
                else
                {
                    data.StatusCode = StatusCodes.Status404NotFound;
                    data.message = "Refresh token expired or invalid refresh token .You have to login again using right credentials.";
                    return data;
                }
            }
            else
            {
                data.StatusCode = StatusCodes.Status202Accepted;
                data.message = "Access Token Can't be Empty.";
                return data;
            }


        }
        #endregion 


        #region Generate Json Web Token By Using UserName and Password
        private JwtSecurityToken GenerateJsonWebToken(LoginDTO _userData)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _userData.Email),
                     new Claim(ClaimTypes.GivenName, _userData.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            var token = GetToken(authClaims);
            return token;
        }


        #endregion

        private JwtSecurityToken GetToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: signinCredentials
            );

        }

        #region Generate Web Token By Using Principal Claim
        private JwtSecurityToken GenerateJsonWebToken(IEnumerable<Claim> claims)
        {

            var tokenString = GetToken(claims);

            return tokenString;

        }

        #endregion

        #region Generate Refresh Token

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        #endregion


        #region Get Principal From Expired Token
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey")),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;

        }

        #endregion 








        #region Reshma 
        public async Task<IActionResult> GetUserIDBasedOnUserName(string userName)
        {
            var jwtToken = await HttpContext.GetTokenAsync("access_token"); 
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["JWT:ValidAudience"].ToLower();
            validationParameters.ValidIssuer = _configuration["JWT:ValidIssuer"].ToLower();
            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            var getdata = _userManager.FindByEmailAsync(userName);
            return Ok();
        }
        #endregion

    }
}
