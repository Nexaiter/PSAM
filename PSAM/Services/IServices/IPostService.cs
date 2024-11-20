using PSAM.DTOs.AccountDTOs;
using PSAM.Entities;

namespace PSAM.Services.IServices
{
    public interface IPostService
    {
        Task CreatePost(int authorId, string title, string content);
        Task DeletePost(int postId);
        Task<List<PostDTO>> GetAllPosts(int pageNumber, int pageSize);
        Task UpdatePost(int postId, UpdatePostDTO post);
        Task<List<PostDTO>> GetSubscribedPosts(int accountId, int pageNumber, int pageSize);
        Task<List<PostDTO>> GetPostsByAccountId(int accountId, int pageNumber, int pageSize);
    }
}
