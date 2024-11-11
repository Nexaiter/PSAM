namespace PSAM.DTOs.AccountDTOs
{
    public class ImageDTO
    {
        public int ImageId { get; set; }
        public int PostId { get; set; }

        // Obraz w formacie Base64
        public string Data { get; set; }
    }
}
