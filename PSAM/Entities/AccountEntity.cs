using System.ComponentModel.DataAnnotations;

namespace PSAM.Entities
{
    public class AccountEntity
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public int SubscriberAmount { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public string City { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        // Kolekcje subskrybentów i subskrybowanych
        public virtual ICollection<SubscribersEntity> Subscribers { get; set; } // Konta subskrybujące to konto
        public virtual ICollection<SubscribersEntity> Subscribing { get; set; } // Konta, które subskrybuje to konto

        
    }
}
