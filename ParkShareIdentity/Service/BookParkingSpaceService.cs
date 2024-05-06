using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nest;
using ParkShareIdentity.Areas.Identity.Data;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Enums;
using ParkShareIdentity.Model;
using ParkShareIdentity.Service.Interface;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace ParkShareIdentity.Service
{
    public class BookParkingSpaceService : IBookParkingSpace
    {
        #region Dependency Injencution
        private readonly IAddSpace _addSpace;
        private readonly IMasterAddress _masterAddress;
        private MasterAddressDTO masterAddressViewModel = new MasterAddressDTO();
        public IConfiguration _configuration;
        private UserManager<ApplicationUser> _userManager;
        private readonly ParkShareIdentityContext _ParkShareIdentityContext;
        ResponseDataDTO responseDataDTO = new ResponseDataDTO();
        private readonly IServer _server;
        private readonly IPayment _payment;
        public BookParkingSpaceService(IAddSpace addSpace, IMasterAddress masterAddress,
            IConfiguration configuration, UserManager<ApplicationUser> userManager,
            ParkShareIdentityContext ParkShareIdentityContext, IServer server, IPayment payment)
        {
            _addSpace = addSpace;
            _masterAddress = masterAddress;
            _configuration = configuration;
            _userManager = userManager;
            _ParkShareIdentityContext = ParkShareIdentityContext;
            _server = server;
            _payment = payment;

        }
        #endregion
        public async Task<ResponseDataDTO> GetAllParkingSpace()
        {
            var addresses = _server.Features.Get<IServerAddressesFeature>().Addresses;
            var imagepath = ((string[])addresses)[1] + "Images\\" ;

            var data = _ParkShareIdentityContext.AddSpaces.Include(x => x.MasterAddress).Where(x => x.IsVacant == true).Select(x => new ParkingSpaceDistanceDTO()
            {
                Id = x.Id,
                AddressId = x.MasterAddress.Id,
                ParkingSpaceAvailability = x.ParkingSpaceAvailablity,
                ZipCode = x.MasterAddress.ZipCode,
                City = x.MasterAddress.City,
                Street = x.MasterAddress.Street,
                Description = x.MasterAddress.Description,
                Longitude = x.MasterAddress.Longitude,
                Latitude = x.MasterAddress.Latitude,
                UserName = x.MasterAddress.ApplicationUser.UserName,
                ImagesList = _ParkShareIdentityContext.Images.Where(y => y.MasterAddressId == x.MasterAddressId).ToList(),
                TimewisedataList = _ParkShareIdentityContext.TimeWiseData.Where(y => y.AddSpaceId == x.Id).ToList()
            }).ToList();

            foreach(var item in data)
            {
                foreach(var image in item.ImagesList)
                {
                    image.Name = imagepath +   image.Name;
                }
            }

            responseDataDTO.StatusCode = StatusCodes.Status200OK;
             responseDataDTO.data = data;
            return responseDataDTO;
        }

        #region Currently Not In Use
        //public async Task<ResponseDataDTO> GetAllParkingSpaceByFilter(BookParkingSpacesDTO bookParkingSpacesDTO)
        //{

        //    var test = (from masteraddress in _ParkShareIdentityContext.MasterAddresses
        //                join addspace in _ParkShareIdentityContext.AddSpaces on masteraddress.Id equals addspace.MasterAddressId
        //                join timewise in _ParkShareIdentityContext.TimeWiseData on addspace.Id equals timewise.AddSpaceId
        //                join images in _ParkShareIdentityContext.Images on timewise.Id equals images.Id
        //                join landlord in _ParkShareIdentityContext.Landlord on masteraddress.Id equals landlord.MasterAddressId
        //                orderby addspace.MasterAddressId
        //                select new
        //                {
        //                    masteraddress.Id,
        //                    masteraddress.ZipCode,
        //                    masteraddress.Street,
        //                    masteraddress.City,
        //                    masteraddress.Description,
        //                    masteraddress.Latitude,
        //                    masteraddress.Longitude,
        //                    timewise.FromDateTime,
        //                    timewise.ToDateTime,
        //                    addspace.IsVacant,
        //                    addspace.Price,
        //                    images.Name,
        //                    landlord.ApplicationUser.UserName
        //                }).Where(x => x.FromDateTime == Convert.ToDateTime(bookParkingSpacesDTO.FromDateTime) &&
        //                         x.ToDateTime == Convert.ToDateTime(bookParkingSpacesDTO.ToDateTime))
        //                        .OrderByDescending(x => x.FromDateTime)
        //                        .OrderByDescending(x => x.ToDateTime).ToList();

        //    responseDataDTO.data = test;
        //    return responseDataDTO;
        //}

        #endregion

        private async Task<ResponseDataDTO> GetAllParkingSpaceByFilter123(BookParkingSpacesBasedOnPlaceAndDateDTO bookParkingSpacesBasedOnPlaceAndDateDTO)
        {
            try
            {
                ResponseDataDTO responseDataDTO = new ResponseDataDTO();
                var b = _ParkShareIdentityContext.AddBooking.ToList();
                var timedata = _ParkShareIdentityContext.TimeWiseData.Where(x => x.FromDateTime == bookParkingSpacesBasedOnPlaceAndDateDTO.Date).ToList();
                var n = "dfd";
                responseDataDTO.data = timedata;
                return responseDataDTO;
            }
            catch(Exception ex)
            {        
                
                responseDataDTO.data = ex;
                return responseDataDTO;
            }
           
        }
        public async Task<ResponseDataDTO> GetAllParkingSpaceByFilter(BookParkingSpacesBasedOnPlaceAndDateDTO bookParkingSpacesBasedOnPlaceAndDateDTO)
        {
            try
            {
                #region testcode
                // var timedata = _ParkShareIdentityContext.TimeWiseData.Select(x => x.FromDateTime == bookParkingSpacesBasedOnPlaceAndDateDTO.Date).FirstOrDefault();
                // var data= bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1
                //    var a="test";
                //    var b ="test";
                //    if (bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1 != null)
                //    {
                //    DateTime dateOnly = bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].From;
                //     a = dateOnly.ToString("yyyy/MM/dd");
                //    DateTime dateOnly1 = bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].To;
                //     b = dateOnly1.ToString("yyyy/MM/dd");
                //}

                //var pl = abc.Where(x => Convert.ToDateTime(x.FromDateTime).ToString("yyyy/MM/dd") == a
                //              && Convert.ToDateTime(x.ToDateTime).ToString("yyyy/MM/dd") == b);
                #endregion
                if (bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1 != null)
                {
                    var date = bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].From;
                    var myData = (from addspace in _ParkShareIdentityContext.AddSpaces
                                  join timewisedata in _ParkShareIdentityContext.TimeWiseData on addspace.Id equals timewisedata.AddSpaceId
                                  join masteraddres in _ParkShareIdentityContext.MasterAddresses on addspace.MasterAddressId equals masteraddres.Id
                                  select new ParkingSpaceDistanceDTO()
                                  {
                                      Id=addspace.Id,
                                      AddressId = masteraddres.Id,
                                      ZipCode = masteraddres.ZipCode,
                                      Street = masteraddres.Street,
                                      City = masteraddres.City,
                                      Description = masteraddres.Description,
                                      Latitude = masteraddres.Latitude,
                                      Longitude = masteraddres.Longitude,
                                      FromDateTime = timewisedata.FromDateTime,
                                      ToDateTime = timewisedata.ToDateTime,
                                      IsVacant = addspace.IsVacant,
                                      Price = addspace.Price,
                                      UserName = masteraddres.ApplicationUser.UserName,
                                      ParkingSpaceAvailability=addspace.ParkingSpaceAvailablity
                                  }).ToList();
                    var bookingData = (from addbooking in _ParkShareIdentityContext.AddBooking
                                       where (addbooking.FromDateTime >= Convert.ToDateTime(bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].From))
                                       && (addbooking.ToDateTime <= Convert.ToDateTime(bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].To))
                                       select new
                                       {
                                            addbooking.AddSpaceId,
                                           addbooking.AddBookingId,
                                           addbooking.FromDateTime,
                                           addbooking.ToDateTime
                                       }).ToList();

                    List<ParkingSpaceDistanceDTO> data = new List<ParkingSpaceDistanceDTO>();
                    //foreach (var item in myData)
                    //{
                    //    var distanceinKM = distance(item.Latitude, (double)bookParkingSpacesBasedOnPlaceAndDateDTO.Lattitude, item.Longitude, (double)bookParkingSpacesBasedOnPlaceAndDateDTO.Longitude);

                    //    if (distanceinKM <= 2)
                    //    {
                    //        data.Add(item);
                    //    }
                    //}

                    #region Reshma Code Logic
                    
                    List<ParkingSpaceDistanceDTO> data1 = new List<ParkingSpaceDistanceDTO>();
                    var x = _ParkShareIdentityContext.TimeWiseData.ToList();
                    var s = x.Where(x =>
                             x.FromDateTime < bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].From &&
                            x. ToDateTime > bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].To);

                    List<TimeWiseData> t = new List<TimeWiseData>();
                    foreach(var c in x)
                    {
                        if((c.FromDateTime < bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].From &&
                            c.ToDateTime > bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].To)
                            ||
                            (c.FromDateTime < bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].From &&
                            c.ToDateTime < bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].From)
                            ||
                            (c.FromDateTime > bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].To &&
                            c.ToDateTime > bookParkingSpacesBasedOnPlaceAndDateDTO.DateTime1[0].To)
                            )
                        {

                        }
                        else
                        {
                            var isExist = _ParkShareIdentityContext.AddBooking.Where(a => a.FromDateTime == c.FromDateTime && a.ToDateTime == c.ToDateTime).ToList();
                            if(isExist.Count == 0)
                            {
                                var masterAddressId = _ParkShareIdentityContext.AddSpaces.FirstOrDefault(x=>x.Id == c.AddSpaceId).MasterAddressId;
                                var spaceData = _ParkShareIdentityContext.AddSpaces.Where(x => x.Id == c.AddSpaceId).FirstOrDefault();
                                var myMaster = _ParkShareIdentityContext.MasterAddresses.Include(x => x.ApplicationUser).Where(x=>x.Id==masterAddressId);
                                var master =  _ParkShareIdentityContext.MasterAddresses.FirstOrDefault(m => m.Id == masterAddressId);
                                double lat = master.Latitude;
                                double lng = master.Longitude;
                                var distanceinKM = distance((double)bookParkingSpacesBasedOnPlaceAndDateDTO.Lattitude, lat, (double)bookParkingSpacesBasedOnPlaceAndDateDTO.Longitude, lng);
                                if (distanceinKM <= 2)
                                {
                                    TimeWiseData t1 = new TimeWiseData();
                                    ParkingSpaceDistanceDTO parkingSpaceDistanceDTO = new ParkingSpaceDistanceDTO();
                                    parkingSpaceDistanceDTO.AddressId = masterAddressId;
                                    parkingSpaceDistanceDTO.Id = c.AddSpaceId;
                                    parkingSpaceDistanceDTO.IsVacant = true;
                                    parkingSpaceDistanceDTO.City = master.City;
                                    parkingSpaceDistanceDTO.Street = master.Street;
                                    parkingSpaceDistanceDTO.ToDateTime = c.ToDateTime;
                                    parkingSpaceDistanceDTO.FromDateTime = c.FromDateTime;
                                    parkingSpaceDistanceDTO.ZipCode = master.ZipCode;
                                    parkingSpaceDistanceDTO.Description = master.Description;
                                    parkingSpaceDistanceDTO.Latitude = master.Latitude;
                                    parkingSpaceDistanceDTO.Longitude = master.Longitude;
                                    parkingSpaceDistanceDTO.Price = spaceData.Price;
                                    parkingSpaceDistanceDTO.ParkingSpaceAvailability = spaceData.ParkingSpaceAvailablity;
                                    parkingSpaceDistanceDTO.IsVacant = spaceData.IsVacant;
                                    parkingSpaceDistanceDTO.UserName = master.ApplicationUser.UserName;
                                    
                                    //   t1 = c;
                                    //  t.Add(c);
                                    data.Add(parkingSpaceDistanceDTO);
                                }   
                            }
                        }
                    }
                    responseDataDTO.data = data;
                   // responseDataDTO.data = t;
                    return responseDataDTO;   

                    #endregion
                    List<ParkingSpaceDistanceDTO> data2 = new List<ParkingSpaceDistanceDTO>();
                    bool test = false;
                    foreach (var item in data)
                    {
                        foreach (var bookedData in bookingData)
                        {
                            if (bookedData.AddSpaceId == item.Id && bookedData.FromDateTime == item.FromDateTime && bookedData.ToDateTime == item.ToDateTime)
                            {
                                test = true;
                            }
                            else
                            {
                                item.ParkingSpaceStatus = false;
                                test = false;
                                // data1.Add(item);
                            }

                        }
                        if (!test)
                        {
                            data2.Add(item);
                        }
                      
                 

                    }

                    responseDataDTO.data = data; 
                    return responseDataDTO;
                }
                else
                {
                    var myData = (from addspace in _ParkShareIdentityContext.AddSpaces
                                  join timewisedata in _ParkShareIdentityContext.TimeWiseData on addspace.Id equals timewisedata.AddSpaceId
                                  join masteraddres in _ParkShareIdentityContext.MasterAddresses on addspace.MasterAddressId equals masteraddres.Id
                                  where Convert.ToDateTime(timewisedata.FromDateTime) == Convert.ToDateTime(bookParkingSpacesBasedOnPlaceAndDateDTO.Date)
                                  select new ParkingSpaceDistanceDTO()
                                  {
                                      Id = masteraddres.Id,
                                      ZipCode = masteraddres.ZipCode,
                                      Street = masteraddres.Street,
                                      City = masteraddres.City,
                                      Description = masteraddres.Description,
                                      Latitude = masteraddres.Latitude,
                                      Longitude = masteraddres.Longitude,
                                      FromDateTime = timewisedata.FromDateTime,
                                      ToDateTime = timewisedata.ToDateTime,
                                      IsVacant = addspace.IsVacant,
                                      Price = addspace.Price,
                                      ParkingSpaceAvailability=addspace.ParkingSpaceAvailablity
                                  }).ToList();


                    List<ParkingSpaceDistanceDTO> data = new List<ParkingSpaceDistanceDTO>();
                    foreach (var item in myData)
                    {
                        var distanceinKM = distance(item.Latitude, (double)bookParkingSpacesBasedOnPlaceAndDateDTO.Lattitude, item.Longitude, (double)bookParkingSpacesBasedOnPlaceAndDateDTO.Longitude);
                        if (distanceinKM <= 2)
                        {
                            data.Add(item);
                        }
                    }
                    responseDataDTO.data = data;
                }
                return responseDataDTO;

            }
            catch (Exception ex)
            {
                responseDataDTO.StatusCode = StatusCodes.Status400BadRequest;
                responseDataDTO.message = ex.Message;
                return responseDataDTO;
            }

        }
     
        public async Task<ResponseDataDTO> GetAllBookingDetailsByUserId(string email)
        {
            LoginDTO login = new LoginDTO();
            var message = _ParkShareIdentityContext.Users.FirstOrDefault(x => x.Email == email);
            //var appUser = await _userManager.FindByEmailAsync(login.Email);
            if (message != null)
            {
                var test = (from masteraddress in _ParkShareIdentityContext.MasterAddresses
                            join addspace in _ParkShareIdentityContext.AddSpaces on masteraddress.Id equals addspace.MasterAddressId
                            join timewise in _ParkShareIdentityContext.TimeWiseData on addspace.Id equals timewise.AddSpaceId
                            join images in _ParkShareIdentityContext.Images on timewise.Id equals images.Id
                            orderby addspace.MasterAddressId
                            select new
                            {
                                masteraddress.Id,
                                masteraddress.ZipCode,
                                masteraddress.Street,
                                masteraddress.City,
                                masteraddress.Description,
                                masteraddress.Latitude,
                                masteraddress.Longitude,
                                timewise.FromDateTime,
                                timewise.ToDateTime,
                                addspace.IsVacant,
                                addspace.Price,
                                images.Name
                            }).ToList();

                responseDataDTO.data = test;
            }
            return responseDataDTO;
        }

        public async Task<ResponseDataDTO> AddBooking(AddBookingDTO addBooking, string UserName)
        {
            try
            {
                AddBooking data = new AddBooking();
                var getUserId = await _userManager.FindByEmailAsync(UserName);
                var alReadyBookedList = _ParkShareIdentityContext.AddBooking.Where(x => x.AddSpaceId == addBooking.AddSpaceId).ToList();
                var getSpace = _ParkShareIdentityContext.AddSpaces.Where(x => x.Id == addBooking.AddSpaceId).FirstOrDefault();

                if (getSpace == null)
                {
                    responseDataDTO.message = "Space not available";
                    return responseDataDTO;
                }
                var timeWseData = _ParkShareIdentityContext.TimeWiseData.Where(x => x.AddSpaceId == addBooking.AddSpaceId).ToList();
                var isVehicleexist = _ParkShareIdentityContext.Vehicles.FirstOrDefault(x => x.VehicleId == addBooking.VehicleId);
                bool flag = false;
                if (alReadyBookedList.Count > 0)
                {
                    foreach (var item in alReadyBookedList)
                    {

                        if ((addBooking.DateTime1[0].To >= item.FromDateTime) && (addBooking.DateTime1[0].To <= item.ToDateTime))
                        {
                            flag = false;
                            responseDataDTO.StatusCode = StatusCodes.Status200OK;
                            responseDataDTO.message = "This time is in use.";
                            return responseDataDTO;
                        }
                        else if ((addBooking.DateTime1[0].From >= item.FromDateTime) && (addBooking.DateTime1[0].From <= item.ToDateTime))
                        {
                            flag = false;
                            responseDataDTO.StatusCode = StatusCodes.Status200OK;
                            responseDataDTO.message = "This time is in use.";
                            return responseDataDTO;
                        }
                        else if (addBooking.DateTime1[0].From < item.FromDateTime && addBooking.DateTime1[0].To > item.FromDateTime)
                        {
                            flag = false;
                            responseDataDTO.StatusCode = StatusCodes.Status200OK;
                            responseDataDTO.message = "This time is in use.";
                            return responseDataDTO;
                        }
                        else
                        {
                            flag = true;
                        }

                    }
                    if (flag)
                    {
                        if (getSpace.ParkingSpaceAvailablity == (int)SpaceAvailability.RentableAtCertainTime)
                        {
                            foreach (var item in timeWseData)
                            {
                                if (addBooking.DateTime1[0].From >= item.FromDateTime && addBooking.DateTime1[0].To <= item.ToDateTime)
                                {
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                if (isVehicleexist != null && getUserId.Id == isVehicleexist.UserId)
                                {
                                    return responseDataDTO = await AddBookingMethod(addBooking, getUserId.Id);
                                }
                                else
                                {
                                    responseDataDTO.StatusCode = StatusCodes.Status406NotAcceptable;
                                    responseDataDTO.data = "Please select proper vehicle.";
                                    return responseDataDTO;
                                }
                            }
                        }
                        else if (getSpace.ParkingSpaceAvailablity == (int)SpaceAvailability.AlwaysOnRent)
                        {
                            if (isVehicleexist != null && getUserId.Id == isVehicleexist.UserId)
                            {
                                return responseDataDTO = await AddBookingMethod(addBooking, getUserId.Id);
                            }
                            else
                            {
                                responseDataDTO.StatusCode = StatusCodes.Status406NotAcceptable;
                                responseDataDTO.data = "Please select proper vehicle.";
                                return responseDataDTO;
                            }
                        }
                    }

                    else
                    {
                        responseDataDTO.StatusCode = StatusCodes.Status406NotAcceptable;
                        responseDataDTO.data = "Please select proper data.";
                        return responseDataDTO;
                    }

                }
                else
                {
                    if (getSpace.ParkingSpaceAvailablity == (int)SpaceAvailability.AlwaysOnRent)
                    {
                        if (isVehicleexist != null && getUserId.Id == isVehicleexist.UserId)
                        {
                            return responseDataDTO = await AddBookingMethod(addBooking, getUserId.Id);
                        }
                        else
                        {
                            responseDataDTO.StatusCode = StatusCodes.Status406NotAcceptable;
                            responseDataDTO.data = "Please select proper vehicle.";
                            return responseDataDTO;
                        }
                    }
                    else if (getSpace.ParkingSpaceAvailablity == (int)SpaceAvailability.RentableAtCertainTime)
                    {
                        foreach (var item in timeWseData)
                        {
                            if (addBooking.DateTime1[0].From == item.FromDateTime && addBooking.DateTime1[0].To == item.ToDateTime)
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            if (isVehicleexist != null && getUserId.Id == isVehicleexist.UserId)
                            {
                                return responseDataDTO = await AddBookingMethod(addBooking, getUserId.Id);
                            }
                            else
                            {
                                responseDataDTO.StatusCode = StatusCodes.Status406NotAcceptable;
                                responseDataDTO.data = "Please select proper vehicle.";
                                return responseDataDTO;
                            }
                        }

                    }
                }
                responseDataDTO.StatusCode = StatusCodes.Status406NotAcceptable;
                responseDataDTO.data = "This  space is not in used or invalid date time.";
                return responseDataDTO;

            }
            catch (Exception ex)
            {
                responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                responseDataDTO.message = ex.Message;
                return responseDataDTO;
            }


        }

        private async Task<ResponseDataDTO> AddBookingMethod(AddBookingDTO addBooking, string userid)
        {
            AddBooking data = new AddBooking();
            var getSpace = _ParkShareIdentityContext.AddSpaces.Where(x => x.Id == addBooking.AddSpaceId).FirstOrDefault();
            var checkBalance = _ParkShareIdentityContext.Wallets.Where(x => x.UserId == userid).FirstOrDefault();
            if (getSpace != null)
            {
                TimeSpan value = addBooking.DateTime1[0].From.Subtract(addBooking.DateTime1[0].To);
                double AbsoluteValue = Math.Abs(value.TotalHours);
                data.Price = getSpace.Price * AbsoluteValue;
            }
            if (checkBalance.WalletBalance >= data.Price)
            {
                data.AddSpaceId = addBooking.AddSpaceId;
                data.VehicleId = addBooking.VehicleId;
                data.UserId = userid;
                //  data.TimeWiseId = addBooking.TimewiseId;
                data.FromDateTime = addBooking.DateTime1[0].From;
                data.ToDateTime = addBooking.DateTime1[0].To;
                _ParkShareIdentityContext.AddBooking.Add(data);
                _ParkShareIdentityContext.SaveChanges();
                responseDataDTO.data= await _payment.PaymentBooking(data.AddBookingId);
                EmailSend(data);
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                responseDataDTO.data = "Parking Space Booked Successfuly.";
                return responseDataDTO;
            }
            else
            {
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                responseDataDTO.message = "Insufficient Balance.";
                return responseDataDTO;
            }
        }

        public async Task<ResponseDataDTO> GetAllParkingHistory()
        {
            //var data = _ParkShareIdentityContext.AddBooking.ToList();
            responseDataDTO.data = (from addbookin in _ParkShareIdentityContext.AddBooking
                                    join vehicle in _ParkShareIdentityContext.Vehicles on addbookin.VehicleId equals vehicle.VehicleId
                                    join addspace in _ParkShareIdentityContext.AddSpaces on addbookin.AddSpaceId equals addspace.Id
                                    join masteraddress in _ParkShareIdentityContext.MasterAddresses on addspace.MasterAddressId equals masteraddress.Id
                                    select new SpaceBookingHistoryDTO()
                                    {
                                        //  masteraddress.Id,
                                        ParkingSpaceOwner = masteraddress.ApplicationUser.UserName,
                                        Zipcode = masteraddress.ZipCode,
                                        Street = masteraddress.Street,
                                        City = masteraddress.City,
                                        Description = masteraddress.Description,
                                        Lattittude = masteraddress.Latitude,
                                        Longitude = masteraddress.Longitude,
                                        IsVacant = addspace.IsVacant,
                                        ParkingSpaceAvailability = addspace.ParkingSpaceAvailablity,
                                        TotalPrice = addbookin.Price,
                                        TotalHours = addbookin.FromDateTime - addbookin.ToDateTime,
                                        FromDateTime = addbookin.FromDateTime,
                                        ToDateTime = addbookin.ToDateTime,
                                        ParkingSpaceRenter = addbookin.applicationuser.UserName,
                                    }).ToList();
            responseDataDTO.StatusCode = StatusCodes.Status200OK;
            return responseDataDTO;
        }

        public async Task<ResponseDataDTO> GetParkingHistoryUserWise(string UserName)
        {
            var getUserId = await _userManager.FindByEmailAsync(UserName);
            if (getUserId != null)
            {
                //   var g = _ParkShareIdentityContext.MasterAddresses.Where(m => m.addSpaces.FirstOrDefault(a => a.addBookings.Select(b=>b.UserId).where( b=>b.UserId == getUserId )));
                //var a = _ParkShareIdentityContext.MasterAddresses.Where(m => m.addSpaces.FirstOrDefault(a =>
                //{
                //    List<bool> list = a.addBookings.Select(b => b.UserId == getUserId.Id).ToList();
                //    return list;
                //}));
                //var alReadyBookedList = 

                var UserInRole = _ParkShareIdentityContext.MasterAddresses.
                             Join(_ParkShareIdentityContext.AddSpaces, m => m.Id, ad => ad.MasterAddressId,
(m, ad) => new { m, ad }).
Join(_ParkShareIdentityContext.AddBooking, r => r.ad.Id, ro => ro.AddSpaceId, (r, ro) => new { r, ro })
.Where(mr => mr.ro.UserId == getUserId.Id)
.Select(mr => new SpaceBookingHistoryDTO
{
    ParkingSpaceOwner = mr.r.m.ApplicationUser.UserName,
    Description = mr.r.m.Description,
    Zipcode = mr.r.m.ZipCode,
    Street = mr.r.m.Street,
    City = mr.r.m.City,
    Lattittude = mr.r.m.Latitude,
    Longitude = mr.r.m.Longitude,
    IsVacant = mr.ro.AddSpace.IsVacant,
    TotalPrice = mr.ro.Price,
    FromDateTime = mr.ro.FromDateTime,
    ToDateTime = mr.ro.ToDateTime,
    TotalHours = mr.ro.ToDateTime - mr.ro.FromDateTime,
    ParkingSpaceRenter = mr.ro.applicationuser.UserName,
    ParkingSpaceAvailability = mr.ro.AddSpace.ParkingSpaceAvailablity

}).ToList();
                List<SpaceBookingHistoryDTO> spaceBookingHistoryDTOs = new List<SpaceBookingHistoryDTO>();
                //foreach (var spaceBookingHistory in alReadyBookedList)
                //{
                //    SpaceBookingHistoryDTO spaceBookingHistoryDTO = new SpaceBookingHistoryDTO();
                //    spaceBookingHistoryDTO.ParkingSpaceOwner = spaceBookingHistory.AddSpaceId.M
                //}
                //foreach (var item in alReadyBookedList)
                //{
                //  SpaceBookingHistoryDTO spaceBookingHistoryDTO = new SpaceBookingHistoryDTO();
                //                    spaceBookingHistoryDTO.  ParkingSpaceOwner = item.AddSpaceI,
                //                    spaceBookingHistoryDTO. Zipcode = masteraddress.ZipCode,
                //                    spaceBookingHistoryDTO. Street = masteraddress.Street,
                //                    spaceBookingHistoryDTO. City = masteraddress.City,
                //                    spaceBookingHistoryDTO.Description = masteraddress.Description,
                //                    spaceBookingHistoryDTO.Lattittude = masteraddress.Latitude,
                //                    spaceBookingHistoryDTO. Longitude = masteraddress.Longitude,
                //                    spaceBookingHistoryDTO.IsVacant = addspace.IsVacant,
                //                    spaceBookingHistoryDTO.ParkingSpaceAvailability = addspace.ParkingSpaceAvailablity,
                //                    spaceBookingHistoryDTO.TotalPrice = addbookin.Price,
                //                    spaceBookingHistoryDTO.TotalHours = addbookin.FromDateTime - addbookin.ToDateTime,
                //                    spaceBookingHistoryDTO.FromDateTime = addbookin.FromDateTime,
                //                    spaceBookingHistoryDTO.ToDateTime = addbookin.ToDateTime,
                //                    spaceBookingHistoryDTO.ParkingSpaceRenter = addbookin.applicationuser.UserName,

                //}

                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                responseDataDTO.data = UserInRole;
                return responseDataDTO;
            }
            responseDataDTO.StatusCode = StatusCodes.Status404NotFound;
            responseDataDTO.message = "User Id Not Found";
            return responseDataDTO;
        }

        #region Find Distance between two co-ordinates
        private double toRadians(double angleIn10thofaDegree)
        {
            // Angle in 10th
            // of a degree
            return (angleIn10thofaDegree *
                           Math.PI) / 180;
        }
        private double distance(double lat1, double lat2, double lon1, double lon2)
        {

            // The math module contains
            // a function named toRadians
            // which converts from degrees
            // to radians.
            lon1 = toRadians(lon1);
            lon2 = toRadians(lon2);
            lat1 = toRadians(lat1);
            lat2 = toRadians(lat2);

            // Haversine formula
            double dlon = lon2 - lon1;
            double dlat = lat2 - lat1;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(dlon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            // Radius of earth in
            // kilometers. Use 3956
            // for miles
            double r = 6371;

            // calculate the result
            return (c * r);
        }


        #endregion


        public async Task<List<GetAllParkingSpaceDTO>> spservice(SqlParameter[] paraObjects)
        {
            try
            {
                GetAllParkingSpaceDTO data = new GetAllParkingSpaceDTO();

                var dataSet = await _ParkShareIdentityContext.GetQueryDatatableAsync(MainClass.GetVerificationList, paraObjects);

                return ConvertDataTable<GetAllParkingSpaceDTO>(dataSet.Tables[0]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            var data = new List<T>();
            for (var index = 0; index < dt.Rows.Count; index++)
            {
                var row = dt.Rows[index];
                var item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (var pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (dr[column.ColumnName] == DBNull.Value)
                        {
                            pro.SetValue(obj, null, null);
                        }
                        else
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }

                    }

                }
            }
            return obj;
        }

        #region Email Send Code
        private async void EmailSend(AddBooking addBooking)
        {
            //var data = _ParkShareIdentityContext.AddSpaces;
            var addressData = _ParkShareIdentityContext.MasterAddresses.Include(x=>x.ApplicationUser);
            BookingEmailDTO bookingEmailDTO = new BookingEmailDTO();
            foreach (var address in addressData)
            {
                if (addBooking.AddSpace.MasterAddressId == address.Id)
                {
                    bookingEmailDTO.ParkingOwner = address.ApplicationUser.UserName;
                    bookingEmailDTO.City = address.City;
                    bookingEmailDTO.Street = address.Street;

                }
            }
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("abpnarola@gmail.com", "uljshtzeaxczdcsw");

            String from = "abpnarola@gmail.com";
            String to = addBooking.applicationuser.Email;
            String subject = "Parking Space Booked Successfully.";
            //string path = HttpContext.Current.Server.MapPath("~/files/sample.html");
            //E:\ReshmaZIpParkShare\ParkShareIdentity\ParkShareIdentity\Controllers
            string resultPath = "E:\\ReshmaZIpParkShare\\ParkShareIdentity\\ParkShareIdentity\\Controllers\\BookingEmailTemplate.html";
            string content = System.IO.File.ReadAllText(resultPath);
            String messageBody = content;
            messageBody = messageBody.Replace("#UserName", addBooking.applicationuser.UserName);
            messageBody = messageBody.Replace("#ParkingOwner", bookingEmailDTO.ParkingOwner);
            messageBody = messageBody.Replace("#City", bookingEmailDTO.City);
            messageBody = messageBody.Replace("#Street", bookingEmailDTO.Street);
            messageBody = messageBody.Replace("#Totalprice", addBooking.Price.ToString());
            messageBody = messageBody.Replace("#FromeDate", addBooking.FromDateTime.ToString());
            messageBody = messageBody.Replace("#ToDate", addBooking.ToDateTime.ToString());
            MailMessage message = new MailMessage(from, to, subject, messageBody);
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
              $"{messageBody} <br> <img src=\"cid:Wedding\">",
              null,
              "text/html"
            );

            message.AlternateViews.Add(htmlView);

            //   try
            //   {
            smtp.Send(message);
            //  }
            //catch (SmtpException ex)
            //{
            //    return ex.Message;
            //}
            //return null;
        }
        #endregion

    }
}
