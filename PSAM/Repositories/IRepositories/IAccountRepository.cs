using PSAM.Entities;

namespace PSAM.Repositories.IRepositories
{
    public interface IAccountRepository
    {
        Task<List<AccountEntity>> GetAllAccounts(int pageNumber, int pageSize);
        Task RegisterAccount(AccountEntity accountEntity);
        Task<string> GetUsername(int accountId);
        Task<int> GetId(string login, string password);
        Task<AccountEntity> GetAccountById(int accountId);
        Task<bool> CheckAccount(string login, string password);
        Task<bool> CheckUsernameExistence(string username);
        Task<bool> CheckLoginExistence(string login);
        Task<bool> CheckAccountExistence(string login);
        Task<string> SaveImage(string base64Image);
        Task<bool> DeleteAccountById(int accountId);
        Task UpdateAccount(AccountEntity account);
    }
}
