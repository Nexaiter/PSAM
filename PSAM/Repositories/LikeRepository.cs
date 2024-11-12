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

        public async Task LikePost(PostLikeEntity postLikeEntity, int postId)
        {
            _appDbContext.PostLikes.Add(postLikeEntity);
            var post = await _appDbContext.Posts.FindAsync(postId);
            if (post != null)
            {
                post.Likes++;
            }
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UnlikePost(PostLikeEntity postLikeEntity)
        {
            _appDbContext.PostLikes.Remove(postLikeEntity);
            var post = await _appDbContext.Posts.FindAsync(postLikeEntity.PostId);
            if (post != null)
            {
                post.Likes--;
            }
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

        public async Task<int> CountLikesForPost(int postId)
        {
            return await _appDbContext.PostLikes.CountAsync(p => p.PostId == postId);
        }

        public async Task<int> CountLikesForComment(int commentId)
        {
            return await _appDbContext.CommentLikes.CountAsync(c => c.CommentId == commentId);
        }
        public async Task<PostEntity> GetPostById(int postId)
        {
            return await _appDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<CommentEntity> GetCommentById(int commentId)
        {
            return await _appDbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task UpdatePost(PostEntity postEntity)
        {
            _appDbContext.Posts.Update(postEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateComment(CommentEntity commentEntity)
        {
            _appDbContext.Comments.Update(commentEntity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
