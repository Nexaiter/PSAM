using System.ComponentModel.DataAnnotations;

namespace PSAM.DTOs.AccountDTOs
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public int SubscriberAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; } = "";
        public List<TechnologyDTOs> Technologies { get; set; }
    }
}
