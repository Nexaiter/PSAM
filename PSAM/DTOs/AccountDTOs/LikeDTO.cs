namespace PSAM.DTOs.AccountDTOs
{
    public class LikeDTO
    {
        public int LikeId { get; set; }
        public int AccountId { get; set; }

        // Polubiony post lub komentarz
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
    }
}
