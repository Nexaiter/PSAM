using PSAM.Entities;

namespace PSAM.Repositories.IRepositories
{
    public interface ILikeRepository
    {
        Task LikePost(PostLikeEntity postLikeEntity);
        Task UnlikePost(PostLikeEntity postLikeEntity);
        Task LikeComment(CommentLikeEntity commentLikeEntity);
        Task UnlikeComment(CommentLikeEntity commentLikeEntity);
        Task<PostLikeEntity> GetLikedPost(int authorId, int postId);
        Task<CommentLikeEntity> GetLikedComment(int authorId, int commentId);
    }
}
