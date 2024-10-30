using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSAM.Entities
{
    public class CommentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public AccountEntity Author { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public PostEntity Post { get; set; }

        // Relacja do rodzica, jeśli komentarz jest odpowiedzią na inny komentarz
        [ForeignKey("ParentComment")]
        public int? CommentId { get; set; }
        public CommentEntity ParentComment { get; set; }

        // Relacja do potencjalnych odpowiedzi na ten komentarz
        public ICollection<CommentEntity> Replies { get; set; } = new List<CommentEntity>();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
