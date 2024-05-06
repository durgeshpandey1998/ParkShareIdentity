using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkShareIdentity.Areas.Identity.Data
{
    public class Images
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MasterAddress")]
        [Required]
        public int MasterAddressId { get; set; }
        public virtual MasterAddress MasterAddress { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public bool IsPreview { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public string Path { get; set; }
    }
}
