using Microsoft.Data.SqlClient;
using ParkShareIdentity.DTO;

namespace ParkShareIdentity.Service.Interface
{
    public interface IBookParkingSpace
    {
      //  Task<ResponseDataDTO> GetAllParkingSpaceByFilter(BookParkingSpacesDTO bookParkingSpacesDTO);
        Task<ResponseDataDTO> GetAllParkingSpace();
        Task<ResponseDataDTO> GetAllBookingDetailsByUserId(string email);
        Task<ResponseDataDTO> AddBooking(AddBookingDTO addBookingDTO,string UserName);
        Task<ResponseDataDTO> GetAllParkingSpaceByFilter(BookParkingSpacesBasedOnPlaceAndDateDTO bookParkingSpacesBasedOnPlaceAndDateDTO);
        Task<ResponseDataDTO> GetAllParkingHistory();
        Task<ResponseDataDTO> GetParkingHistoryUserWise(string UserName);
        Task<List<GetAllParkingSpaceDTO>> spservice(SqlParameter[] paraObjects);
    }
}
