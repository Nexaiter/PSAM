using System.ComponentModel.DataAnnotations.Schema;

namespace PSAM.Entities
{
    public class SubscribersEntity
    {
        [ForeignKey("SubscriberAccount")]
        public int SubscriberId { get; set; } // Konto, które subskrybuje

        [ForeignKey("SubscribeeAccount")]
        public int SubscribeeId { get; set; } // Konto, które jest subskrybowane

        // Nawigacyjne właściwości
        public virtual AccountEntity SubscriberAccount { get; set; }
        public virtual AccountEntity SubscribeeAccount { get; set; }
    }
}
