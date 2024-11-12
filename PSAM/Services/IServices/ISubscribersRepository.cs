using PSAM.Entities;

namespace PSAM.Services.IServices
{
    public interface ISubscribersRepository
    {
        Task Subscribe(SubscribersEntity subscribe);
        Task Unsubscribe(int accountId, int subscribeeId);
        Task<List<AccountEntity>> GetAccountsSubscribers(int accountId, int pageNumber, int pageSize);
        Task<bool> AlreadySubbed(int accountId, int subscribeeId);
        Task<List<AccountEntity>> GetAccountsSubscriptions(int accountId, int pageNumber, int pageSize);
        Task<int> GetSubscriberAmount(int accountId);
    }
}
