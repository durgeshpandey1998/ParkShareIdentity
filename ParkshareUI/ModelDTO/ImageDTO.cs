using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkshareUI.ModelDTO
{
    public class ImageDTO
    {
        public int Id { get; set; }

   
        [Required]
        public int MasterAddressId { get; set; }

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
