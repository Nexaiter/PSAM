using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSAM.Entities
{
    public class LikeEntity
    {
        [Key]
        public int LikeId { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual AccountEntity Account { get; set; }

        public int? PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual PostEntity Post { get; set; }

        public int? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public virtual CommentEntity Comment { get; set; }
    }
}
