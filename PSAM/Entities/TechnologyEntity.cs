using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSAM.Entities
{
    public class TechnologyEntity
    {
        [Key]
        public int TechnologyId { get; set; }

        // Tylko jeden klucz obcy do AccountEntity
        [ForeignKey("AccountEntity")]
        public int AccountId { get; set; }

        [MaxLength(50)]
        public string Technology { get; set; }

        public virtual AccountEntity Account { get; set; }
    }
}
