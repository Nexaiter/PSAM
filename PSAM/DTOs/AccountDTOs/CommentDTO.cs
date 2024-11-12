using PSAM.Entities;

namespace PSAM.DTOs.AccountDTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public int Likes { get; set; }



        // Autor komentarza
        public AccountDTO Author { get; set; }

        public int? PostId { get; set; }

        // Id rodzica, jeśli jest odpowiedzią na inny komentarz
        public int? ParentCommentId { get; set; } // Zmieniono nazwę na bardziej opisową

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Lista odpowiedzi na ten komentarz
        public List<CommentDTO> Replies { get; set; } = new List<CommentDTO>();
        public virtual ICollection<CommentLikeEntity> CommentLikes { get; set; }
    }
}
