using ParkShareIdentity.Data;
using ParkShareIdentity.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class AddSpace
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [EnumDataType(typeof(SpaceAvailability), ErrorMessage = "Invalid SpaceAvailability")]
        [Required]
        public int ParkingSpaceAvailablity { get; set; }
        
        [EnumDataType(typeof(PreviewTimeInWeeks), ErrorMessage = "Invalid PreviewTimeInWeeks")]
        [Required]
        public int PreviewTimeInWeeks { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+(?:\.[0-9]{1,1})?$", ErrorMessage = "Price can have only one precision")]
        [Range(0.1, double.PositiveInfinity, ErrorMessage = "Price must be greater than 0.1 CHF")]
        public double Price { get; set; }
        //public int Day { get; set; }
        public bool IsVacant { get; set; }

        //[ForeignKey("Landlord")]
        //[Required]
        //public int LandlordId { get; set; }
        //public virtual Landlord Landlord { get; set; }

        [ForeignKey("MasterAddress")]
        [Required]
        public int MasterAddressId { get; set; }
        public virtual MasterAddress MasterAddress { get; set; }
        
      // public ICollection<AddBooking> addBookings { get; set; }
        public AddSpace()
        {
            IsVacant = true;
        }

    }
}
