using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<PostEntity>> GetAllPosts(int pageNumber, int pageSize)
        {
            return await _appDbContext.Posts
                .Include(p => p.PostLikes)
                .Include(p => p.Author)
                .Include(p => p.Author.Technologies)
                .Include(p => p.Comments)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task CreatePost(PostEntity postEntity)
        {
            _appDbContext.Posts.Add(postEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeletePost(int postId)
        {
            var postEntity = await _appDbContext.Posts.FindAsync(postId);
            _appDbContext.Posts.Remove(postEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdatePost(PostEntity postEntity)
        {
            _appDbContext.Posts.Update(postEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<PostEntity> GetPostById(int postId)
        {
            var post = await _appDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            return post;
        }

        public async Task<List<PostEntity>> GetPostsBySubscribedAccounts(List<int> subscribedAccountIds, int pageNumber, int pageSize)
        {
            return await _appDbContext.Posts
                .Include(p => p.PostLikes)
                .Include(p => p.Author)
                .Where(post => subscribedAccountIds.Contains(post.AuthorId))
                .OrderByDescending(post => post.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<PostEntity>> GetPostsByAccountId(int accountId, int pageNumber, int pageSize)
        {
            return await _appDbContext.Posts
                .Include(p => p.PostLikes)
                .Include(p => p.Author)
                .Where(p => p.AuthorId == accountId)
                .OrderByDescending(p => p.CreatedAt) // Jeśli chcesz sortować np. po dacie
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
