using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using ParkShareIdentity.Data;
using ParkShareIdentity.DTO;
using ParkShareIdentity.Service.Interface;
using ParkShareIdentity.Enums;
using ParkShareIdentity.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace ParkShareIdentity.Service
{
    public class PaymentService : IPayment
    {
        #region Dependency Injencution
        private readonly IAddSpace _addSpace;
        private readonly IMasterAddress _masterAddress;
        private MasterAddressDTO masterAddressViewModel = new MasterAddressDTO();
        public IConfiguration _configuration;
        private UserManager<ApplicationUser> _userManager;
        private readonly ParkShareIdentityContext _ParkShareIdentityContext;
        private readonly IServer _server;


        public PaymentService(IAddSpace addSpace, IMasterAddress masterAddress,
            IConfiguration configuration, UserManager<ApplicationUser> userManager,
            ParkShareIdentityContext ParkShareIdentityContext, IServer server)
        {
            _addSpace = addSpace;
            _masterAddress = masterAddress;
            _configuration = configuration;
            _userManager = userManager;
            _ParkShareIdentityContext = ParkShareIdentityContext;
            _server = server;

        }
        #endregion

        public async Task<ResponseDataDTO> Recharge(RechargeDTO rechargeDTO,string UserName)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var getUserId= await _userManager.FindByEmailAsync(UserName);
            var walletAccount = _ParkShareIdentityContext.Wallets.Where(x=>x.UserId==getUserId.Id && x.IsActive==true).FirstOrDefault();
            if (walletAccount != null)
            {
                var response1 = UpdateWalletBalance(rechargeDTO, getUserId.Id);
                if (response1 != "success")
                {
                    responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                    responseDataDTO.message = "Internal Server Error in Wallet ";
                    return responseDataDTO;
                }
                responseDataDTO.message = "Recharege Success";
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                return responseDataDTO;
            }
            else
            {
                var response = AddWalletBalance(rechargeDTO, getUserId.Id);
                if (response != "success")
                {
                    responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                    responseDataDTO.message = "Internal Server Error in Wallet ";
                    return responseDataDTO;
                }
                response = InsertRecordInTransaction(rechargeDTO, getUserId.Id);
                if (response != "success")
                {
                    responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                    responseDataDTO.message = "Internal Server Error Transaction";
                    return responseDataDTO;
                }
                responseDataDTO.message = "Recharege Success";
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                return responseDataDTO;
            }
            return responseDataDTO;
        }
        private string AddWalletBalance(RechargeDTO rechargeDTO,string userId)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            try
            {
                Wallet wallet = new Wallet();
                wallet.ActivateDate = DateTime.UtcNow;
                wallet.UserId = userId;
                wallet.IsActive = true;
                wallet.WalletBalance = rechargeDTO.Amount;
                _ParkShareIdentityContext.Wallets.Add(wallet);
                _ParkShareIdentityContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                responseDataDTO.message = ex.Message;
                return string.Empty;
            }

        }

        private string UpdateWalletBalance(RechargeDTO rechargeDTO, string userId)
        {
            ResponseDataDTO responseDataDTO= new ResponseDataDTO();
            var walletAccount = _ParkShareIdentityContext.Wallets.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
            try
            {
                walletAccount.WalletBalance = walletAccount.WalletBalance + rechargeDTO.Amount;
                _ParkShareIdentityContext.Wallets.Update(walletAccount);
                _ParkShareIdentityContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {

                responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                responseDataDTO.message = ex.Message;
                return string.Empty;
            }
        }
        private string InsertRecordInTransaction(RechargeDTO rechargeDTO,string userId)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var walletId = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.UserId == userId);
            Guid obj = Guid.NewGuid();
            try
            {
                Transaction transaction = new Transaction();
                transaction.Amount=rechargeDTO.Amount;
                transaction.WalletId = walletId.WalletId;
                transaction.TransactionDate = DateTime.Now;
                transaction.Type= (int)TransactionType.Recharge;
                transaction.ReferenceNo = obj.ToString();
                _ParkShareIdentityContext.Transactions.Add(transaction);
                _ParkShareIdentityContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                responseDataDTO.message=ex.Message;
                return string.Empty;
            }
        }
        public async Task<ResponseDataDTO> GetWalletBalance(string UserName)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            try
            {
                GetUserBalanceDTO getUserBalanceDTO = new GetUserBalanceDTO();
                var getUser = await _userManager.FindByEmailAsync(UserName);
                var walletBalance = _ParkShareIdentityContext.Wallets.Where(x => x.UserId == getUser.Id).ToList();
                foreach (var item in walletBalance)
                {
                    getUserBalanceDTO.WalletId = item.WalletId;
                    getUserBalanceDTO.WalletBalance = item.WalletBalance;
                    getUserBalanceDTO.UserName = item.ApplicationUser.UserName;
                    getUserBalanceDTO.ActiveDate = item.ActivateDate;
                    getUserBalanceDTO.Status = item.IsActive;

                }
                responseDataDTO.data=getUserBalanceDTO;
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                return responseDataDTO;
            }
            catch (Exception ex)
            {

                return responseDataDTO;
            }
         
        }



        public async Task<ResponseDataDTO>PaymentBooking(int BookingId)
        {
            ResponseDataDTO responseDataDTO =new ResponseDataDTO();
            Transaction transaction = new Transaction();
            var spaceId = _ParkShareIdentityContext.AddBooking.FirstOrDefault(x => x.AddBookingId == BookingId);
            if (spaceId==null)
            {
                responseDataDTO.message = "No record found";
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                return responseDataDTO;
            }
            int masterAddressId = _ParkShareIdentityContext.AddSpaces.FirstOrDefault(p => p.Id == spaceId.AddSpaceId).MasterAddressId;
            var id = _ParkShareIdentityContext.MasterAddresses.FirstOrDefault(x => x.Id == masterAddressId);
            Guid obj = Guid.NewGuid();
            var walletDataRenter = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.UserId == spaceId.UserId);
            transaction.ReferenceNo = obj.ToString();
            transaction.TransactionDate = DateTime.Now;
            transaction.Type = (int)TransactionType.Book_Debit;
            transaction.WalletId = walletDataRenter.WalletId;
            transaction.Amount = spaceId.Price;
            transaction.BookingId = BookingId;
            var response = InsertTransactionForRenter(transaction);
            if (response != "success")
            {
                responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                responseDataDTO.message = "Internal Server Error in Wallet ";
                return responseDataDTO;
            }
            Transaction transactionowner = new Transaction();
            Guid guid = Guid.NewGuid();
            var walletDataOwner = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.UserId == id.UserId);
            transactionowner.ReferenceNo = guid.ToString();
            transactionowner.TransactionDate= DateTime.Now;
            transactionowner.Type=(int)TransactionType.Book_Credit;
            transactionowner.WalletId = walletDataOwner.WalletId;
            transactionowner.Amount= spaceId.Price;
            transactionowner.BookingId = BookingId;
            response = InsertTransactionForOwner(transactionowner);
            if (response != "success")
            {
                responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                responseDataDTO.message = "Internal Server Error in Wallet ";
                return responseDataDTO;
            }
            responseDataDTO.message = "Payment Success";
            responseDataDTO.StatusCode = StatusCodes.Status200OK;
            return responseDataDTO;
        }

        private string InsertTransactionForRenter(Transaction transaction)
        {
            try
            {
               // Wallet wallet = new Wallet();
                var walletData = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.WalletId == transaction.WalletId);
                walletData.WalletBalance=walletData.WalletBalance - transaction.Amount;
                _ParkShareIdentityContext.Transactions.Add(transaction);
                _ParkShareIdentityContext.Wallets.Update(walletData);
                _ParkShareIdentityContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private string InsertTransactionForOwner(Transaction transaction)
        {
            try
            {
                var walletData = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.WalletId == transaction.WalletId);
                walletData.WalletBalance = walletData.WalletBalance + transaction.Amount;
                _ParkShareIdentityContext.Transactions.Add(transaction);
                _ParkShareIdentityContext.Update(walletData);
                _ParkShareIdentityContext.SaveChanges();
                return "success";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public async Task<ResponseDataDTO>CancelBooking(int BookingId,string principal)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var currentLoginUser = await _userManager.FindByEmailAsync(principal);
            var checkCancelData = _ParkShareIdentityContext.Transactions.FirstOrDefault(x => x.BookingId == BookingId && x.Type == (int)TransactionType.Cancel_Debit || x.Type == (int)TransactionType.Cancel_Credit);
            var walletDataForRenter = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.UserId == currentLoginUser.Id);
            var transactionDataForRenter = _ParkShareIdentityContext.Transactions.FirstOrDefault(x => x.WalletId == walletDataForRenter.WalletId && x.BookingId == BookingId);
            var TransactionDataForOwner = _ParkShareIdentityContext.Transactions.FirstOrDefault(x => x.Type == (int)TransactionType.Book_Credit && x.BookingId == BookingId);
            //   if (TransactionDataForOwner!=null
            // {
            if (TransactionDataForOwner==null)
            {
                responseDataDTO.message = "Booking Id is not valid";
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                return responseDataDTO;
            }
                var walletDataForOwner = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.WalletId == TransactionDataForOwner.WalletId);
            if (walletDataForOwner.WalletBalance<TransactionDataForOwner.Amount)
            {
                responseDataDTO.StatusCode = StatusCodes.Status502BadGateway;
                responseDataDTO.message = "Insufficient Balance.";
                return responseDataDTO;
            }
          //  }
            if (walletDataForRenter.UserId != currentLoginUser.Id || checkCancelData!=null)
            {
                responseDataDTO.StatusCode = StatusCodes.Status401Unauthorized;
                responseDataDTO.message = "You are not Authorized User for this transaction or This space already Canceled";
                return responseDataDTO;
            }
            else
            {
                Transaction transaction = new Transaction();
                Guid obj = Guid.NewGuid();
                transaction.ReferenceNo = obj.ToString();
                transaction.TransactionDate = DateTime.Now;
                transaction.Type = (int)TransactionType.Cancel_Credit;
                transaction.WalletId = walletDataForRenter.WalletId;
                transaction.Amount = transactionDataForRenter.Amount;
                transaction.BookingId = BookingId;
                var response = CancelTransactionForRenter(transaction);
                if (response != "success")
                {
                    responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                    responseDataDTO.message = "Internal Server Error in Wallet ";
                    return responseDataDTO;
                }
                Transaction transactionowner = new Transaction();
                Guid guid = Guid.NewGuid();
                transactionowner.ReferenceNo = guid.ToString();
                transactionowner.TransactionDate = DateTime.Now;
                transactionowner.Type = (int)TransactionType.Cancel_Debit;
                transactionowner.WalletId = walletDataForOwner.WalletId;
                transactionowner.Amount = TransactionDataForOwner.Amount;
                transactionowner.BookingId = BookingId;
                response = CancelTransactionForOwner(transactionowner);
                if (response != "success")
                {
                    responseDataDTO.StatusCode = StatusCodes.Status500InternalServerError;
                    responseDataDTO.message = "Internal Server Error in Wallet ";
                    return responseDataDTO;
                }
                responseDataDTO.message = "Booking Canceled Successfully";
                responseDataDTO.StatusCode = StatusCodes.Status200OK;
                return responseDataDTO;
            }
        }

        private string CancelTransactionForRenter(Transaction transaction)
        {
            try
            {
                // Wallet wallet = new Wallet();
                var walletData = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.WalletId == transaction.WalletId);
                walletData.WalletBalance = walletData.WalletBalance + transaction.Amount;
                _ParkShareIdentityContext.Transactions.Add(transaction);
                _ParkShareIdentityContext.Wallets.Update(walletData);
                _ParkShareIdentityContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        private string CancelTransactionForOwner(Transaction transaction)
        {
            try
            {
                var walletData = _ParkShareIdentityContext.Wallets.FirstOrDefault(x => x.WalletId == transaction.WalletId);
                walletData.WalletBalance = walletData.WalletBalance - transaction.Amount;
                _ParkShareIdentityContext.Transactions.Add(transaction);
                _ParkShareIdentityContext.Update(walletData);
                _ParkShareIdentityContext.SaveChanges();
                return "success";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<ResponseDataDTO> TransactionHistory(string UserName)
        {
            ResponseDataDTO responseDataDTO = new ResponseDataDTO();
            var userData = await  _userManager.FindByEmailAsync(UserName);
            if (userData == null)
            {
                responseDataDTO.message = "User not found";
                responseDataDTO.StatusCode = StatusCodes.Status404NotFound;
                return responseDataDTO;
            }
            var walletData = _ParkShareIdentityContext.Wallets.Where(x => x.UserId == userData.Id).FirstOrDefault();
            var transactionData = _ParkShareIdentityContext.Transactions.Where(x => x.WalletId == walletData.WalletId).ToList();
            responseDataDTO.data = transactionData;
            responseDataDTO.StatusCode = StatusCodes.Status200OK;
            return responseDataDTO;
        }

    }
}
