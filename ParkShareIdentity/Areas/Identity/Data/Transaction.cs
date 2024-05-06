using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // public int TransactionId { get; set; }
        public int Id { get; set; }
        public double Amount { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Type { get; set; }

      //  [ForeignKey("AddBooking")]
        public int? BookingId { get; set; }
      //  public virtual AddBooking AddBooking { get; set; }

        [ForeignKey("Wallet")]
        [Required]
        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
    }
}
