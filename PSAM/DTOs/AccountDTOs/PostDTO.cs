using PSAM.Entities;

namespace PSAM.DTOs.AccountDTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public int Likes { get; set; }


        public int AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<PostLikeEntity> PostLikes { get; set; } = new List<PostLikeEntity>();

        // Lista komentarzy
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
