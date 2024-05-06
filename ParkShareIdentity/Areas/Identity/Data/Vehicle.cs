using ParkShareIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class Vehicle
    {
        [Key]

        public int VehicleId { get; set; }
        [ForeignKey("applicationuser")]
        public  string UserId { get; set; }       
           public  virtual ApplicationUser applicationuser { get; set; }
        public string?  VehicleType { get; set; }
       public  string? VehicleNumber { get; set; }
    }
}
