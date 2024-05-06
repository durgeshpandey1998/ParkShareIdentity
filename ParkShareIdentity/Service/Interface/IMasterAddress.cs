using ParkShareIdentity.DTO;

namespace ParkShareIdentity.Service.Interface
{
    public interface IMasterAddress
    {
        dynamic AddMasterAddress(MasterAddressDTO data);
        Task<ResponseDataDTO> UpdateMasterAddress(MasterAddressUpdateDTO data);
       Task<ResponseDataDTO> GetMasterAddressList(string principal);
        Task<ResponseDataDTO> EditMasterAddress(int masterAddressId);
    }
}
