using Microsoft.EntityFrameworkCore;
using PSAM.Entities;
using PSAM.Repositories.IRepositories;

namespace PSAM.Repositories
{
    public class TechnologyRepository : ITechnologyRepository
    {
        public readonly AppDbContext _appDbContext;

        public TechnologyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }

        public async Task AddTechnology(int accountId, string technology)
        {
            var tech = new TechnologyEntity
            {
                AccountId = accountId,
                Technology = technology
            };
            _appDbContext.Technologies.Add(tech);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task RemoveTechnology(int technologyId)
        {
            var tech = await _appDbContext.Technologies.FindAsync(technologyId);
            if(tech != null)
            {
                _appDbContext.Technologies.Remove(tech);
                await _appDbContext.SaveChangesAsync();
            }

        }

        public async Task<List<TechnologyEntity>> GetAccountTechnologies(int accountId)
        {
            return await _appDbContext.Technologies
                .Where(t => t.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<bool> AccountHasTechnology(int accountId, string tech)
        {
            return await _appDbContext.Technologies
                .AnyAsync(t => t.AccountId == accountId && t.Technology == tech);
        }
    }
}
