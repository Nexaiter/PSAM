using Microsoft.AspNetCore.Razor.TagHelpers;
using PSAM.Entities;
using PSAM.Repositories.IRepositories;

namespace PSAM.Repositories
{
    public class PostRepository : IPostRepository
    {
        public readonly AppDbContext _appDbContext;

        public PostRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
