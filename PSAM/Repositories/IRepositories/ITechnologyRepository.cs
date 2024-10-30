using PSAM.Entities;

namespace PSAM.Repositories.IRepositories
{
    public interface ITechnologyRepository
    {
        Task<bool> AccountHasTechnology(int accountId, string tech);
        Task<List<TechnologyEntity>> GetAccountTechnologies(int accountId);
        Task RemoveTechnology(int technologyId);
        Task AddTechnology(int accountId, string technology);
    }
}
