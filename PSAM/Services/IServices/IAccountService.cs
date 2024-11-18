using PSAM.DTOs.AccountDTOs;
using PSAM.Entities;
using PSAM.Models;

namespace PSAM.Services.IServices
{
    public interface IAccountService
    {
        Task<bool> CheckAccount(string login, string password);
        Task<bool> CheckAccountExistence(string login);
        Task<List<AccountDTO>> GetAllAccounts(int pageNumber, int pageSize);

        Task<int> GetId(string login, string password);
        Task<AccountDTO> GetPlayerById(int accountId);
        Task<string> GetUsername(int accountId);
        Task RegisterAccount(RegisterModel registerModel);
        Task DeleteAccountById(int accountId);
        Task UpdateById(int accountId, AccountUpdateDTO updateDTO);
        Task<List<TechnologyDTOs>> GetAccountTechs(int accountId);
        Task RemoveTechnology(int technologyId);
        Task AddTechnology(int accountId, string technology);
        Task<List<AccountDTO>> GetAccountsSubscriptions(int accountId, int pageNumber, int pageSize);
        Task<List<AccountDTO>> GetAccountsSubscribers(int accountId, int pageNumber, int pageSize);
        Task Unsubscribe(int accountId, int subscribeeId);
        Task Subscribe(int accountId, int subscribeeId);
        Task<int> GetSubscriberAmount(int accountId);
        Task UpdateProfileImage(int accountId, string base64Image);
        Task DeleteProfileImage(int accountId);
        Task<List<AccountDTO>> GetFilteredAccounts(int pageNumber, int pageSize, string? username, string? firstName, string? lastName, string? city);
    }
}