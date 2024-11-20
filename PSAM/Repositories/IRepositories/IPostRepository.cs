using PSAM.Entities;

namespace PSAM.Repositories.IRepositories
{
    public interface IPostRepository
    {
        Task CreatePost(PostEntity postEntity);
        Task DeletePost(int postId);
        Task UpdatePost(PostEntity postEntity);
        Task<List<PostEntity>> GetAllPosts(int pageNumber, int pageSize);
        Task<PostEntity> GetPostById(int postId);
        Task<List<PostEntity>> GetPostsBySubscribedAccounts(List<int> subscribedAccountIds, int pageNumber, int pageSize);
        Task<List<PostEntity>> GetPostsByAccountId(int accountId, int pageNumber, int pageSize);
    }
}
