using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSAM.Entities
{
    public class ImageEntity
    {
        [Key]
        public int ImageId { get; set; }

        public int PostId { get; set; }
        public PostEntity Post { get; set; }

        [Required]
        public string Data { get; set; } // Przechowywanie obrazu w formacie Base64
    }
}
