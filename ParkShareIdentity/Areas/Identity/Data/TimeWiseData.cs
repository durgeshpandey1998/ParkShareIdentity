using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class TimeWiseData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime FromDateTime { get; set; }

        [Required]
        public DateTime ToDateTime { get; set; }

        [Required]
        [ForeignKey("AddSpace")]        
        public int AddSpaceId { get; set; }
        public virtual AddSpace AddSpace { get; set; }
        
    }
}
