using ParkShareIdentity.Enums;
using System.ComponentModel.DataAnnotations;

namespace ParkShareIdentity.DTO
{
    public class RechargeDTO
    {
        [Required]
        public double Amount { get; set; }

      //  [EnumDataType(typeof(TransactionType))]
       // public  TransactionType TransactionType { get; set; }
    }
}
