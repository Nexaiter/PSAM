using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSAM.Entities
{
    public class SubscribersEntity
    {
        [Key]
        public int Id { get; set; } // Primary key for this entity

        [ForeignKey("SubscriberAccount")]
        public int SubscriberId { get; set; } // Account that is subscribing

        [ForeignKey("SubscribeeAccount")]
        public int SubscribeeId { get; set; } // Account that is being subscribed to

        // Navigation properties
        public virtual AccountEntity SubscriberAccount { get; set; } // Reference to the subscriber account
        public virtual AccountEntity SubscribeeAccount { get; set; } // Reference to the subscribed account
    }
}
