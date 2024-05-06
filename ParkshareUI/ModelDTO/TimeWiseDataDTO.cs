using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkshareUI.ModelDTO
{
    public class TimeWiseDataDTO
    {
        public int Id { get; set; }

        [Required]
        public DateTime FromDateTime { get; set; }

        [Required]
        public DateTime ToDateTime { get; set; }

        [Required]

        public int AddSpaceId { get; set; }
        
    }
}
