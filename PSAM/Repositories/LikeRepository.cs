using Microsoft.EntityFrameworkCore;
using PSAM.Entities;
using PSAM.Repositories.IRepositories;
using PSAM.Services;

namespace PSAM.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _appDbContext;

        public LikeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task LikePost(PostLikeEntity postLikeEntity)
        {
            _appDbContext.PostLikes.Add(postLikeEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UnlikePost(PostLikeEntity postLikeEntity)
        {
            _appDbContext.PostLikes.Remove(postLikeEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task LikeComment(CommentLikeEntity commentLikeEntity)
        {
            _appDbContext.CommentLikes.Add(commentLikeEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UnlikeComment(CommentLikeEntity commentLikeEntity)
        {
            _appDbContext.CommentLikes.Remove(commentLikeEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<PostLikeEntity> GetLikedPost(int authorId, int postId)
        {
            return await _appDbContext.PostLikes
                .FirstOrDefaultAsync(p => p.AccountId == authorId && p.PostId == postId);
        }
        public async Task<CommentLikeEntity> GetLikedComment(int authorId, int commentId)
        {
            return await _appDbContext.CommentLikes
                .FirstOrDefaultAsync(p => p.AccountId == authorId && p.CommentId == commentId);
        }
    }
}
