using ParkShareIdentity.DTO;

namespace ParkShareIdentity.Service.Interface
{
    public interface IPayment
    {
        Task<ResponseDataDTO> Recharge(RechargeDTO rechargeDTO, string UserName);
        Task<ResponseDataDTO> GetWalletBalance(string UserName);
        Task<ResponseDataDTO> PaymentBooking(int BookingId);
        Task<ResponseDataDTO> CancelBooking(int BookingId,string UserName);
        Task<ResponseDataDTO> TransactionHistory(string UserName);
    }
}
