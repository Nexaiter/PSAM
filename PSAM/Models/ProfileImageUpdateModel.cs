using System.ComponentModel.DataAnnotations;

namespace PSAM.Models
{
    public class ProfileImageUpdateModel
    {
        [Required]
        public string Base64Image { get; set; }
    }

}
