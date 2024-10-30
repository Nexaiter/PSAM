using PSAM.Entities;
using PSAM.Repositories.IRepositories;

namespace PSAM.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        public readonly AppDbContext _appDbContext;

        public CommentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
