using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ParkShareIdentity.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkShareIdentity.Shared
{
    public class JWTToken
    {
        private readonly IConfiguration _config;
        public IConfiguration _configuration;
        public JWTToken(IConfiguration config, IConfiguration configuration)
        {
            _config = config;
            _configuration = configuration;
        }


        #region Generate JWT Token
        /*public string GenerateJSONWebToken(RegisterModel userInfo)
        {

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userInfo.Email),
                     new Claim(ClaimTypes.GivenName, userInfo.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey"));
            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/
        #endregion

        public string GetEmailBasedOnToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["JWT:ValidAudience"].ToLower();
            validationParameters.ValidIssuer = _configuration["JWT:ValidIssuer"].ToLower();
            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            var userName = principal.Identities.ToList()[0].Name;

            return userName;
        }


    }
}
