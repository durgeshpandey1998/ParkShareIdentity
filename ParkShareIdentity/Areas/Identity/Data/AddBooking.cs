using ParkShareIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class AddBooking
    {
        [Key]
        public int AddBookingId { get; set; }
        [ForeignKey("Vehicles")]
        public int? VehicleId { get; set; }
        public virtual Vehicle Vehicles { get; set; }
        [ForeignKey("applicationuser")]
        public string UserId { get; set; }
        public virtual ApplicationUser applicationuser { get; set; }
        [ForeignKey("AddSpace")]
        public int ?AddSpaceId { get; set; }
        public virtual AddSpace AddSpace { get; set; }
        [Required]
        public DateTime FromDateTime { get; set; }
        [Required]
        public DateTime ToDateTime { get; set; }
        public double Price { get; set; }

    }
}
