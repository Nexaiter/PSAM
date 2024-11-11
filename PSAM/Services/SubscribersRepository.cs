using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PSAM.Entities;
using PSAM.Services.IServices;

namespace PSAM.Services
{
    public class SubscribersRepository : ISubscribersRepository
    {
        public readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;


        public SubscribersRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;

        }

        public async Task Subscribe(SubscribersEntity subscribe)
        {
            _appDbContext.Subscribers.Add(subscribe);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Unsubscribe(int accountId, int subscribeeId)
        {
            var sub = await _appDbContext.Subscribers
            .FirstOrDefaultAsync(s => s.SubscriberId == accountId && s.SubscribeeId == subscribeeId);

            if (sub != null)
            {
                _appDbContext.Subscribers.Remove(sub);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<AccountEntity>> GetAccountsSubscriptions(int accountId, int pageNumber, int pageSize)
        {
            return await _appDbContext.Subscribers
                .Where(s => s.SubscriberId == accountId)
                .Select(s => s.SubscribeeAccount)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<AccountEntity>> GetAccountsSubscribers(int accountId, int pageNumber, int pageSize)
        {
            return await _appDbContext.Subscribers
                .Where(s => s.SubscribeeId == accountId)
                .Select(s => s.SubscriberAccount)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<bool> AlreadySubbed(int accountId, int subscribeeId)
        {
            return await _appDbContext.Subscribers
                .AnyAsync(s => s.SubscriberId == accountId && s.SubscribeeId == subscribeeId);
        }


    }
}
