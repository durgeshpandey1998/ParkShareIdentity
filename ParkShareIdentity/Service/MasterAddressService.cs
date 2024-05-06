using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkShareIdentity.Areas.Identity.Data;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Response;
using ParkShareIdentity.Service.Interface;

namespace ParkShareIdentity.Service
{
    public class MasterAddressService : IMasterAddress
    {
        private readonly ParkShareIdentityContext _ParkShareIdentityContext;
        private readonly IConfiguration _config;
        private UserManager<ApplicationUser> _userManager;
        DynamicResponse dr = new DynamicResponse();
        public MasterAddressService(ParkShareIdentityContext ParkShareIdentityContext, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _ParkShareIdentityContext = ParkShareIdentityContext;
            _config = config;
            _userManager = userManager;
        }
        public dynamic AddMasterAddress(MasterAddressDTO data)
        {
            try
            {
                MasterAddress obj = new MasterAddress();
                obj.ZipCode = data.ZipCode;
                obj.Street = data.Street;
                obj.City = data.City;
                obj.Latitude = Convert.ToDouble(data.Latitude);
                obj.Longitude = data.Longitude;
                obj.JsonString = data.JsonString;
                obj.NoOfPictures = data.NoOfPictures.Value;
                obj.Description = data.Description;
                obj.UserId = data.UserId;
                var result = _ParkShareIdentityContext.MasterAddresses.FirstOrDefault(x => x.Latitude == data.Latitude && x.Longitude == data.Longitude);
                if (result != null)
                {
                    return dr.Ok("Latitude and Longitude already exist");
                }
                _ParkShareIdentityContext.MasterAddresses.Add(obj);
                _ParkShareIdentityContext.SaveChanges();
                return dr.Ok(200, "Success", obj);
            }
            catch (Exception ex)
            {
                var jsonData = new
                {
                    data = ex
                };
                return dr.Ok(jsonData);
            }

        }
        public async Task<ResponseDataDTO> UpdateMasterAddress(MasterAddressUpdateDTO data)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            MasterAddress masterAddress = new MasterAddress();
            var isAddressBooked = _ParkShareIdentityContext.AddBooking.Include(x => x.AddSpace).Select(x => new
            {
                Id = x.AddSpace.MasterAddressId,
                Addspaceid = x.AddSpaceId,
                FromDate = x.FromDateTime
            }).ToList();
            var result = _ParkShareIdentityContext.MasterAddresses.FirstOrDefault(x => x.Id == data.Id);
            bool flag = true;

            foreach (var item in isAddressBooked)
            {
                if (result.Id == item.Id && item.FromDate > DateTime.Now)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                if (result.UserId == data.UserId)
                {
                   result.UserId = data.UserId;
                 //  result.Id = result.Id;
                   result.Longitude = data.Longitude;
                   result.Latitude = data.Latitude;
                   result.City = data.City;
                   result.Description = data.Description;
                   result.Street = data.Street;
                   result.ZipCode = data.ZipCode;
                   result.JsonString = data.JsonString;
                   result.NoOfPictures = result.NoOfPictures;
                    _ParkShareIdentityContext.MasterAddresses.Update(result);
                     _ParkShareIdentityContext.SaveChanges();
                    responseDataDTO.StatusCode = StatusCodes.Status200OK;
                    responseDataDTO.message = "Address Updated Successfully.";

                }
                else
                {
                    responseDataDTO.message = "Invalid User";
                    return responseDataDTO;
                }
            }
            else
            {
                responseDataDTO.data = StatusCodes.Status200OK;
                responseDataDTO.message = "You can not update this addres.This address is booked for vehicle parking.";
                return responseDataDTO;
            }
            //   responseDataDTO.data = masterAddressDTOs;
            return responseDataDTO;
        }

        public async Task<ResponseDataDTO> EditMasterAddress(int masterAddressId)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var result = _ParkShareIdentityContext.MasterAddresses.Where(x => x.Id == masterAddressId);
            responseDataDTO.data = result;
            return responseDataDTO;
        }
        public async Task<ResponseDataDTO> GetMasterAddressList(string principal)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            List<MasterAddressUpdateDTO> masterAddressDTOs = new List<MasterAddressUpdateDTO>();
            var getUserId = await _userManager.FindByEmailAsync(principal);
            var addressData = _ParkShareIdentityContext.MasterAddresses.Where(x => x.UserId == getUserId.Id);
            foreach (var item in addressData)
            {
                MasterAddressUpdateDTO data = new MasterAddressUpdateDTO();
                data.UserId = item.UserId;
                data.Id = item.Id;
                data.Longitude = item.Longitude;
                data.Latitude = item.Latitude;
                data.City = item.City;
                data.Description = item.Description;
                data.Street = item.Street;
                data.ZipCode = item.ZipCode;
                masterAddressDTOs.Add(data);
            }
            responseDataDTO.data = masterAddressDTOs;
            return responseDataDTO;
        }
    }
}
