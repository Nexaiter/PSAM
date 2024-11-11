using PSAM.Entities;

namespace PSAM.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        Task<List<CommentEntity>> GetAllComments(int pageNumber, int pageSize);
        Task<List<CommentEntity>> GetAllCommentsFromPost(int postId, int pageNumber, int pageSize);
        Task<List<CommentEntity>> GetAllCommentsFromComment(int commentId, int pageNumber, int pageSize);

        Task CreateComment(CommentEntity commentEntity);
        Task DeleteComment(int commentId);
        Task UpdateComment(CommentEntity newComment);
        Task<CommentEntity> GetCommentById(int commentId);
        Task<CommentEntity> GetCommentWithReplies(int commentId);
    }
}
