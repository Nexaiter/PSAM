using PSAM.DTOs.AccountDTOs;

namespace PSAM.Services.IServices
{
    public interface ICommentService
    {
        Task CreateComment(int authorId, string text, int? postId = null, int? parentCommentId = null);
        Task<List<CommentDTO>> GetAllCommentsFromComment(int commentId, int pageNumber, int pageSize);
        Task<List<CommentDTO>> GetAllCommentsFromPost(int postId, int pageNumber, int pageSize);
        Task<List<CommentDTO>> GetAllComments(int pageNumber, int pageSize);
        Task UpdateComment(int commentId, string text);
        Task DeleteComment(int commentId);
    }
}
