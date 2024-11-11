using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PSAM.Entities
{
    public class CommentLikeEntity
    {
        [Key]
        public int LikeId { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual AccountEntity Account { get; set; }

        public int? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public virtual CommentEntity Comment { get; set; }
    }
}
