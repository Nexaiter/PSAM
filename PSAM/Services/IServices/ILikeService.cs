using PSAM.DTOs.AccountDTOs;

namespace PSAM.Services.IServices
{
    public interface ILikeService
    {
        Task LikePost(int authorId, int postId);
        Task UnlikePost(int postId);
        Task LikeComment(int authorId, int commentId);
        Task UnlikeComment(int commentId);
        Task<int> GetPostLikesCount(int postId);
        Task<int> GetCommentLikesCount(int commentId);
    }
}
