using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Model;

namespace ParkShareIdentity.Service.Interface
{
    public interface IRegisterService
    {
        string EmailSend(RegisterModel userData);
        Task<ResponseDataDTO> RegisterUser(RegisterModel userData);
        Task<ResponseDataDTO> LoginService(LoginDTO _userData);
        Task<ResponseDataDTO> ResetPassword(ResetPasswordDTO _userData);
        Task<ResponseDataDTO> GetUserProfile(string UserName);
        Task<ResponseDataDTO> ChangeAddress(AddressDTO addressData, string userName);
        Task<ResponseDataDTO> ChangePassword(ChangePasswordDTO _userData, string userName);
        Task<ResponseDataDTO> DeleteUser(string userName);
        Task<ResponseDataDTO> ConfirmationEmail(ConfirmationEmailDTO _userData);
        Task<ResponseDataDTO> ForgotPassword(ForgotPasswordDTO _userData);
        Task<ResponseDataDTO> RefreshToken(RefreshTokenDTO refreshTokenDTO);


    }
}
