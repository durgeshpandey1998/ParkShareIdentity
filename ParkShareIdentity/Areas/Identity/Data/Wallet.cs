using ParkShareIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class Wallet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WalletId { get; set; }
        public double WalletBalance { get; set; }
        public DateTime ActivateDate { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("ApplicationUser")]
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public Wallet()
        {
            IsActive = true;
        }
    }
}
