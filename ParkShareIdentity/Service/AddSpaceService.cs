using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkShareIdentity.Areas.Identity.Data;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Enums;
using ParkShareIdentity.Response;
using ParkShareIdentity.Service.Interface;

namespace ParkShareIdentity.Service
{
    public class AddSpaceService : IAddSpace
    {
        private readonly IMasterAddress _masterAddress;
        private readonly ParkShareIdentityContext _ParkShareIdentityContext;
        private readonly IConfiguration _config;
        DynamicResponse dr = new DynamicResponse();
        private MasterAddressDTO masterAddressViewModel = new MasterAddressDTO();
        // private Microsoft.AspNetCore.Hosting.IHostingEnvironment _Environment;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IServer _server;
        private UserManager<ApplicationUser> _userManager;
        public AddSpaceService(ParkShareIdentityContext ParkShareIdentityContext,
            IConfiguration config,
            IMasterAddress masterAddress, IWebHostEnvironment hostEnvironment,
            IServer server, UserManager<ApplicationUser> userManager)
        {
            _ParkShareIdentityContext = ParkShareIdentityContext;
            _config = config;
            _masterAddress = masterAddress;
            webHostEnvironment = hostEnvironment;
            _server = server;
            _userManager = userManager;
        }
        public dynamic ValidateImagesViewModel(ImagesViewModelDTO data)
        {
            #region
            if (data.Pictures == null)
            {
                return dr.Ok("Please upload image");
            }
            if (data.IsPreview == null)
            {
                return dr.Ok("Please set preview");
            }
            if (data.Order == null)
            {
                return dr.Ok("Please set order");
            }
            //validation on formdata and total pictures
            if (data.Pictures.Count > 5)
            {
                return dr.Ok("Maximum 5 image can be upload");
            }
            string[] contentTypeArr = { "image/png", "image/jpg", "image/jpeg" };
            for (int i = 0; i < (data.Pictures.Count); i++)
            {
                if (!contentTypeArr.Contains(data.Pictures.ToList()[i].ContentType))
                {
                    return dr.Ok("Please select png, jpg or jpeg file only");
                }
            }
            #endregion
            if (!(data.Pictures.Count == data.IsPreview.Count && data.Pictures.Count == data.Order.Count))
            {
                return dr.Ok("total images, total preview , total order not matches");
            }

            int isPreviewCount = 0;
            for (int i = 0; i < (data.Pictures.Count); i++)
            {
                if (data.IsPreview.ToList()[i] == true && isPreviewCount == 1)
                {
                    return dr.Ok("multiple true value for preview");
                }
                if (data.IsPreview.ToList()[i] == true)
                {
                    isPreviewCount++;
                }
            }
            if (isPreviewCount == 0)
            {
                return dr.Ok("Please set preview for any one ");
            }
            var r = data.Order.ToArray();
            Array.Sort(r);
            int[] arr = new int[data.Pictures.Count];

            for (int i = 0; i < (data.Pictures.Count); i++)
            {
                arr[i] = i + 1;
            }
            for (int i = 0; i < (data.Pictures.Count); i++)
            {
                if (arr[i] != r[i])
                {
                    return dr.Ok("Order is not valid");
                }
            }
            var result = AddImages(data);
            if (result != string.Empty)
            {
                return dr.Ok(result);
            }
            return dr.Ok(data);
        }
        public void saveImage(ImagesViewModelDTO data)
        {
            int cnt = 0;
            foreach (var file in data.Pictures)
            {
                cnt++;
                // string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "ParkShare.BL/Images");
                //string path = _Environment.WebRootPath + "Images\\" + "aftercrop.png";
                string path = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                //get userid by email
                string fileName = file.FileName; //data.MasterAddressId + "_" + data.UserId + "_" + cnt + ".JPG";

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }
        public void updateMasterAddress(int id, int totalImage)
        {
            MasterAddress masterAddress = new MasterAddress();

            masterAddress = _ParkShareIdentityContext.MasterAddresses.FirstOrDefault(x => x.Id == id);
            masterAddress.NoOfPictures = totalImage;
            _ParkShareIdentityContext.MasterAddresses.Update(masterAddress);
            _ParkShareIdentityContext.SaveChanges();
        }
        public dynamic AddImages(ImagesViewModelDTO data)
        {
            var result = _ParkShareIdentityContext.Images.FirstOrDefault(x => x.MasterAddressId == data.MasterAddressId);
            if (result != null)
            {
                return "Invalid MasterAddressId";
            }
            int totalImage = data.Pictures.Count();

            updateMasterAddress(data.MasterAddressId, totalImage);
            saveImage(data);

            List<Images> lstImages = new List<Images>();

            for (int i = 0; i < totalImage; i++)
            {
                var addresses = _server.Features.Get<IServerAddressesFeature>().Addresses;
                //  var imagepath = ((string[])addresses)[1] + "Images\\" + data.Pictures.ToList()[i].FileName ;
                var imagepath = "Images\\" + data.Pictures.ToList()[i].FileName;
                Images images = new Images();
                images.IsPreview = data.IsPreview.ToList()[i];  //arr[i];
                images.MasterAddressId = data.MasterAddressId;
                images.Type = data.Pictures.ToList()[i].ContentType;
                images.Name = data.Pictures.ToList()[i].FileName; //data.MasterAddressId + "_" + data.UserId + "_" + (i + 1);
                images.Order = data.Order.ToList()[i];  //arr[i];
                images.Path = " ";
                lstImages.Add(images);
            }
            _ParkShareIdentityContext.Images.AddRange(lstImages);
            _ParkShareIdentityContext.SaveChanges();
            return string.Empty;
        }

        public async Task<ResponseDataDTO> AddSpaceData(AddSpaceDTO data, string UserName)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var result = _ParkShareIdentityContext.MasterAddresses.FirstOrDefault(x => x.Id == data.MasterAddressId);
            var isAddressExist = _ParkShareIdentityContext.AddSpaces.Where(x => x.MasterAddressId == data.MasterAddressId).ToList();
            int[] parkingSpaceIds = isAddressExist.Select(x => x.Id).ToArray();

            if (isAddressExist == null)
            {
                responseDataDTO.message = "Address Not belongs to you.";
                return responseDataDTO;
            }
            var getTimewiseData = _ParkShareIdentityContext.TimeWiseData.Where(x => parkingSpaceIds.Contains(x.AddSpaceId)).ToList();
            var userData = await _userManager.FindByEmailAsync(UserName);
            bool flag = true;
            bool flag1 = true;
            if (result != null)
            {
                if (userData.Id != result.UserId)
                {
                    responseDataDTO.message = "Provided addres is not belongs to you.";
                    responseDataDTO.StatusCode = StatusCodes.Status200OK;
                    return responseDataDTO;
                }
            }
            else
            {
                if (data.DateTime1 != null)
                {
                    if (data.DateTime1.Count > 0)
                    {
                        for (int i = 0; i < data.DateTime1.Count; i++)
                        {
                            for (int j = i + i + 1; j < data.DateTime1.Count; j++)
                            {
                                if ((data.DateTime1[j].To >= Convert.ToDateTime(data.DateTime1[i].From)) && (data.DateTime1[j].To <= data.DateTime1[i].To))
                                {
                                    flag1 = false;
                                    var myData = data.DateTime1[j].To.TimeOfDay;
                                    responseDataDTO.message = "Invalid time1";
                                    return responseDataDTO;
                                }
                                if ((data.DateTime1[j].From >= data.DateTime1[i].From) && (data.DateTime1[j].From <= data.DateTime1[i].To))
                                {
                                    flag1 = false;
                                    responseDataDTO.message = "Invalid time2";
                                    return responseDataDTO;
                                }
                            }
                        }
                    }
                    foreach (var item in getTimewiseData)
                    {
                        if (data.DateTime1.Count > 0)
                        {
                            foreach (var dateDTO in data.DateTime1)
                            {
                                if (dateDTO.From == item.FromDateTime && dateDTO.To == item.ToDateTime)
                                {
                                    flag1 = false;
                                    responseDataDTO.message = "Given Date Time Already Exist";
                                    return responseDataDTO;
                                }
                            }
                        }
                    }
                }
            }

            if (flag1)
            {
                if ((int)SpaceAvailability.RentableAtCertainTime == data.ParkingSpaceAvailablity)
                {
                    if (data.DateTime1 == null)
                    {
                        responseDataDTO.message = ("Datetime1 property can't be null when you choose RentableAtCertainTime ");
                        return responseDataDTO;
                    }
                    foreach (var i in data.DateTime1)
                    {
                        if (DateTime.Compare(i.From, i.To) >= 0)
                        {
                            responseDataDTO.message = ("From date can't be greater than To date or can't be same");
                            return responseDataDTO;
                        }
                    }
                    #region Example of code
                    //User Add Multiple time at one click
                    /*  example   "dateTime1": [
                     {
                         "from": "2023-01-09T13:21:38.594Z",
                         "to": "2023-01-09T15:21:38.594Z"
                     },
                     {
                         "from": "2023-01-09T16:21:38.594Z",
                            "to": "2023-01-09T17:21:38.594Z"
                       }
                                                          ], */
                    #endregion
                    bool flag3 = true;
                    if (data.DateTime1.Count > 1)
                    {
                        for (int i = 0; i < data.DateTime1.Count; i++)
                        {
                            for (int j = i + i + 1; j < data.DateTime1.Count; j++)
                            {
                                if ((data.DateTime1[j].To >= Convert.ToDateTime(data.DateTime1[i].From)) && (data.DateTime1[j].To <= data.DateTime1[i].To))
                                {
                                    flag3 = false;
                                    var myData = data.DateTime1[j].To.TimeOfDay;
                                    responseDataDTO.message = "Invalid time1";
                                    return responseDataDTO;
                                }
                                if ((data.DateTime1[j].From >= data.DateTime1[i].From) && (data.DateTime1[j].From <= data.DateTime1[i].To))
                                {
                                    flag3 = false;
                                    responseDataDTO.message = "Invalid time2";
                                    return responseDataDTO;
                                }
                            }
                        }
                    }
                    else
                    {
                        Addspace(data);
                        responseDataDTO.StatusCode = StatusCodes.Status200OK;
                        responseDataDTO.message = "Space addedd successfully.";
                        return responseDataDTO;
                    }
                    if (flag)
                    {
                        Addspace(data);
                        responseDataDTO.message = "Space addedd successfully.";
                        return responseDataDTO;
                    }
                }
                else if ((int)SpaceAvailability.AlwaysOnRent == data.ParkingSpaceAvailablity)
                {
                    if (data.ParkingSpaceAvailablity==(int)SpaceAvailability.AlwaysOnRent && isAddressExist!=null)
                    {
                        responseDataDTO.StatusCode = StatusCodes.Status200OK;
                        responseDataDTO.message = "This space is always on rent, You can not add again.";
                        return responseDataDTO;
                    }
                    Addspace(data);
                    responseDataDTO.StatusCode = StatusCodes.Status200OK;
                    responseDataDTO.message = "Space addedd successfully.";
                    return responseDataDTO;
                }
                else
                {
                    Addspace(data);
                    responseDataDTO.StatusCode = StatusCodes.Status200OK;
                    responseDataDTO.message = "Space addedd successfully.";
                    return responseDataDTO;
                }
            }
            //try
            //{
               return responseDataDTO;
            //}
            //catch (Exception ex)
            //{
            //    responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
            //    responseDataDTO.message = ex.Message;
            //    return responseDataDTO;
            //}
        }
        public dynamic ValidateAddSpaceViewModel(AddSpaceDTO data)
        {
            try
            {
                var result = _ParkShareIdentityContext.MasterAddresses.FirstOrDefault(x => x.Id == data.MasterAddressId);
                var isAddressExist = _ParkShareIdentityContext.AddSpaces.FirstOrDefault(x => x.MasterAddressId == data.MasterAddressId);
                var getTimewiseData = _ParkShareIdentityContext.TimeWiseData.Where(x => x.AddSpaceId == isAddressExist.Id).ToList();
                bool flag1 = true;
                if (data.DateTime1.Count > 0)
                {
                    for (int i = 0; i < data.DateTime1.Count; i++)
                    {
                        for (int j = i + i + 1; j < data.DateTime1.Count; j++)
                        {
                            if ((data.DateTime1[j].To >= Convert.ToDateTime(data.DateTime1[i].From)) && (data.DateTime1[j].To <= data.DateTime1[i].To))
                            {
                                flag1 = false;
                                var myData = data.DateTime1[j].To.TimeOfDay;
                                return dr.Ok("Invalid time1");
                            }
                            if ((data.DateTime1[j].From >= data.DateTime1[i].From) && (data.DateTime1[j].From <= data.DateTime1[i].To))
                            {
                                flag1 = false;
                                return dr.Ok("Invalid time2");
                            }
                        }
                    }
                }

                if (isAddressExist != null)
                {

                    foreach (var item in getTimewiseData)
                    {
                        if (data.DateTime1.Count > 0)
                        {
                            foreach (var dateDTO in data.DateTime1)
                            {
                                if (dateDTO.From == item.FromDateTime && dateDTO.To == item.ToDateTime)
                                {
                                    flag1 = false;
                                }
                            }
                        }
                        else
                        {
                            if (data.DateTime1[0].From == item.FromDateTime && data.DateTime1[0].To == item.ToDateTime)
                            {
                                flag1 = false;
                            }
                        }
                    }
                }
                else
                {
                    if (result == null)
                    {
                        return dr.Ok("MasterAddress Id not exists");
                    }
                    //Please check it its not working properly.....

                    #region Rentable at certain time.......not working
                    if ((int)SpaceAvailability.RentableAtCertainTime == data.ParkingSpaceAvailablity)
                    {
                        if (data.DateTime1 == null)
                        {
                            return dr.Ok("Datetime1 property can't be null when you choose RentableAtCertainTime ");
                        }
                        foreach (var i in data.DateTime1)
                        {
                            if (DateTime.Compare(i.From, i.To) >= 0)
                            {
                                return dr.Ok("From date can't be greater than To date or can't be same");
                            }
                        }
                        #region Example of code
                        //User Add Multiple time at one click
                        /*  example   "dateTime1": [
                         {
                             "from": "2023-01-09T13:21:38.594Z",
                             "to": "2023-01-09T15:21:38.594Z"
                         },
                         {
                             "from": "2023-01-09T16:21:38.594Z",
                                "to": "2023-01-09T17:21:38.594Z"
                           }
                                                              ], */
                        #endregion 
                        bool flag = true;
                        if (data.DateTime1.Count > 1)
                        {
                            for (int i = 0; i < data.DateTime1.Count; i++)
                            {
                                for (int j = i + i + 1; j < data.DateTime1.Count; j++)
                                {
                                    if ((data.DateTime1[j].To >= Convert.ToDateTime(data.DateTime1[i].From)) && (data.DateTime1[j].To <= data.DateTime1[i].To))
                                    {
                                        flag = false;
                                        var myData = data.DateTime1[j].To.TimeOfDay;
                                        return dr.Ok("Invalid time1");
                                    }
                                    if ((data.DateTime1[j].From >= data.DateTime1[i].From) && (data.DateTime1[j].From <= data.DateTime1[i].To))
                                    {
                                        flag = false;
                                        return dr.Ok("Invalid time2");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Addspace(data);
                            return dr.Ok("Space addedd Successfully");
                        }
                        if (flag)
                        {
                            Addspace(data);
                            return dr.Ok("Space addedd successfully.");
                        }
                    }
                    #endregion
                    else if ((int)SpaceAvailability.AlwaysOnRent == data.ParkingSpaceAvailablity)
                    {
                        Addspace(data);
                    }
                    else
                    {
                        Addspace(data);
                        return dr.Ok("Space addedd Successfully");
                    }
                }
                return dr.Ok("Success");
            }
            catch (Exception ex)
            {
                return dr.Ok(100, "", ex);
            }
        }
        public dynamic checkTimeWiseData(AddSpaceDTO data)
        {
            var c = _ParkShareIdentityContext.AddSpaces.Where(x => x.MasterAddressId == data.MasterAddressId).ToList();

            if (c == null)
            {
                var response = Addspace(data);
                return new JsonResult(response);
            }
            else
            {
                try
                {
                    int[] arr = new int[c.Count];
                    for (int i = 0; i < c.Count; i++)
                    {
                        arr[i] = c[i].Id;
                    }
                    //foreach(var l in c)
                    //{ 
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var g = _ParkShareIdentityContext.TimeWiseData.Where(x => x.AddSpaceId == arr[i]).ToList();

                        for (int k = 0; k < g.Count; k++)
                        {
                            var u = g[k].Id;
                            for (int j = 0; j < data.DateTime1.Count; j++)
                            {
                                foreach (var item in data.DateTime1)
                                {

                                }
                                //if ((data.DateTime1[j].To >= g[k].FromDateTime) && (data.DateTime1[j].To <= g[k].ToDateTime))
                                //{
                                //    return dr.Ok("Time already in use");
                                //}
                                //else if ((data.DateTime1[j].From >= g[k].FromDateTime) && (data.DateTime1[j].From <= g[k].ToDateTime))
                                //{
                                //    return dr.Ok("Time already in use");
                                //}
                                //else if (data.DateTime1[j].From < g[k].FromDateTime && data.DateTime1[j].To > g[k].FromDateTime)
                                //{
                                //    return dr.Ok("Time already in use");
                                //}
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex;
                }
                return null;
            }
        }
        public dynamic Addspace(AddSpaceDTO data)
        {
            AddSpace obj = new AddSpace();
            try
            {
                obj.ParkingSpaceAvailablity = data.ParkingSpaceAvailablity;
                obj.PreviewTimeInWeeks = data.PreviewTimeInWeeks;
                obj.Price = data.Price;
                obj.MasterAddressId = (int)data.MasterAddressId;
                if ((int)SpaceAvailability.NotForRentAtTheMoment == data.ParkingSpaceAvailablity)
                {
                    obj.IsVacant = false;
                }
                else
                {
                    obj.IsVacant = true;
                }

                _ParkShareIdentityContext.AddSpaces.Add(obj);
                _ParkShareIdentityContext.SaveChanges();

                #region 
                if ((int)SpaceAvailability.RentableAtCertainTime == data.ParkingSpaceAvailablity)
                {
                    List<TimeWiseData> lstTimeWiseData = new List<TimeWiseData>();

                    foreach (var i in data.DateTime1)
                    {
                        TimeWiseData timeWiseData = new TimeWiseData();
                        timeWiseData.AddSpaceId = obj.Id;
                        timeWiseData.FromDateTime = i.From;
                        timeWiseData.ToDateTime = i.To;
                        lstTimeWiseData.Add(timeWiseData);

                    }
                    _ParkShareIdentityContext.TimeWiseData.AddRange(lstTimeWiseData);
                    _ParkShareIdentityContext.SaveChanges();
                }
                #endregion
                return data;
            }
            catch (Exception ex)
            {
                return ex;//delete all entry
            }
            return string.Empty;

        }
        public dynamic UpdateTime(AddSpaceDTO data)
        {
            try
            {
                return dr.Ok(" Time Updated");
            }
            catch (Exception ex)
            {

                return dr.Ok(200, ex.Message);
            }
        }

        public async Task<ResponseDataDTO> DeleteSpace(int SpaceId, string UserName)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var currentUserData = await _userManager.FindByNameAsync(UserName);
            var myData = _ParkShareIdentityContext.AddSpaces.Include(x => x.MasterAddress).ToList();
            //var addressData = _ParkShareIdentityContext.AddSpaces.Include(x => x.MasterAddress).Select(x=> new
            //{
            //    Id=x.Id,
            //    datalist=_ParkShareIdentityContext.MasterAddresses.Where(y=>y.Id==x.MasterAddressId).ToList(),
            //}).ToList();
            var getParkSpace = _ParkShareIdentityContext.AddSpaces.FirstOrDefault(x => x.Id == SpaceId);
            var bookingData = _ParkShareIdentityContext.AddBooking.FirstOrDefault(x => x.AddSpaceId == SpaceId);
            if (bookingData == null && getParkSpace != null && getParkSpace.IsVacant == true
                                    && getParkSpace.MasterAddress.ApplicationUser.Id == currentUserData.Id)
            {
                getParkSpace.ParkingSpaceAvailablity = 1;
                _ParkShareIdentityContext.Update(getParkSpace);
                _ParkShareIdentityContext.SaveChanges();
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                responseDataDTO.message = "Parking Space Deleted Successfully.";

            }
            else
            {
                responseDataDTO.message = "You can not delete parking space.This parking space is either not available or in use.";
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                return responseDataDTO;
            }
            return responseDataDTO;
        }

    }
}
