using ParkShareIdentity.Areas.Identity.Data;
using ParkShareIdentity.DTO;

namespace ParkShareIdentity.Service.Interface
{
    public interface IVehiclePlates
    {
        Task<ResponseDataDTO> AddVehiclePlate(VehicleDTO vehicleDTO, string UserName);
        Task<ResponseDataDTO> UpdateVehiclePlates(VehicleUpdateDTO vehicleDTO);
        Task<ResponseDataDTO> DeleteVehicles(int Vid);
        Task<ResponseDataDTO> GetVehicleById(int Vid);
        Task<ResponseDataDTO> GetAllVehicle();
        Task<ResponseDataDTO> GetVehicleByUserId(string UserId);
    }
}
