using PSAM.Entities;

namespace PSAM.Repositories.IRepositories
{
    public interface ILikeRepository
    {
        Task LikePost(PostLikeEntity postLikeEntity, int postId);
        Task UnlikePost(PostLikeEntity postLikeEntity);
        Task LikeComment(CommentLikeEntity commentLikeEntity);
        Task UnlikeComment(CommentLikeEntity commentLikeEntity);
        Task<PostLikeEntity> GetLikedPost(int authorId, int postId);
        Task<CommentLikeEntity> GetLikedComment(int authorId, int commentId);
        Task<int> CountLikesForPost(int postId);
        Task<int> CountLikesForComment(int commentId);
        Task<PostEntity> GetPostById(int postId);
        Task<CommentEntity> GetCommentById(int commentId);
        Task UpdatePost(PostEntity postEntity);
        Task UpdateComment(CommentEntity commentEntity);
    }
}
