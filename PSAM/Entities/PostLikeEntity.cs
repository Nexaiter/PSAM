using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PSAM.Entities
{
    public class PostLikeEntity
    {
        [Key]
        public int LikeId { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual AccountEntity Account { get; set; }

        public int? PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual PostEntity Post { get; set; }
    }
}
