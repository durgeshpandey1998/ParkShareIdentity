using Microsoft.AspNetCore.Mvc;
using ParkShareIdentity.DTO;

namespace ParkShareIdentity.Service.Interface
{
    public interface IAddSpace
    {
        dynamic Addspace(AddSpaceDTO data);
        dynamic AddImages(ImagesViewModelDTO data);
        dynamic ValidateAddSpaceViewModel(AddSpaceDTO data);
        dynamic ValidateImagesViewModel(ImagesViewModelDTO data);
        dynamic UpdateTime(AddSpaceDTO data);
        Task<ResponseDataDTO> DeleteSpace(int SpaceId,String UserName);
        Task<ResponseDataDTO> AddSpaceData(AddSpaceDTO data, string UserName);
    }
}
