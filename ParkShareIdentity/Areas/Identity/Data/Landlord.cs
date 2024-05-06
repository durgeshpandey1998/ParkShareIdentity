using ParkShareIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class Landlord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("MasterAddress")]
        [Required]
        public int MasterAddressId { get; set; }
        public virtual MasterAddress MasterAddress { get; set; }

        [ForeignKey("ApplicationUser")]
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public bool IsActive { get; set; }
    }
}
