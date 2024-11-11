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
        
    }
}
