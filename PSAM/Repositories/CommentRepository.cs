using Microsoft.EntityFrameworkCore;
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

        public async Task<List<CommentEntity>> GetAllComments(int pageNumber, int pageSize)
        {
            return await _appDbContext.Comments
                .Include(x => x.Author)
                .Include(x => x.Author.Technologies)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<CommentEntity>> GetAllCommentsFromPost(int postId, int pageNumber, int pageSize)
        {
            return await _appDbContext.Comments
                .Include(x => x.Author)
                .Where(p => p.PostId == postId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<CommentEntity>> GetAllCommentsFromComment(int commentId, int pageNumber, int pageSize)
        {
            return await _appDbContext.Comments
                .Include(x => x.Author)
                .Where(p => p.CommentId == commentId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task CreateComment(CommentEntity commentEntity)
        {
            _appDbContext.Comments.Add(commentEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteComment(int commentId)
        {
            var commentEntity = await _appDbContext.Comments.FindAsync(commentId);
            _appDbContext.Comments.Remove(commentEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateComment(CommentEntity newComment)
        {
            _appDbContext.Comments.Update(newComment);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetCommentById(int commentId)
        {
            var comment = await _appDbContext.Comments.FirstOrDefaultAsync(p => p.Id == commentId);
            return comment;
        }

        public async Task<CommentEntity> GetCommentWithReplies(int commentId)
        {
            return await _appDbContext.Comments
                .Include(x => x.Author)
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

    }
}
