using System.ComponentModel.DataAnnotations;

namespace PSAM.DTOs.AccountDTOs
{
    public class AccountUpdateDTO
    {
        public string? FirstName { get; set; } 

        public string? LastName { get; set; } 
        public string? City { get; set; } 

        public string? Description { get; set; } 

    }
}
