namespace PSAM.DTOs.AccountDTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // Autor posta
        public AccountDTO Author { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Likes { get; set; }

        // Lista komentarzy
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
