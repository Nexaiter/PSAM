using AutoMapper;
using PSAM.DTOs.AccountDTOs;
using PSAM.Entities;
using PSAM.Exceptions;
using PSAM.Repositories.IRepositories;
using PSAM.Services.IServices;

namespace PSAM.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISubscribersRepository _subscribersRepository;

        public PostService(IMapper mapper, IPostRepository postRepository, IAccountRepository accountRepository, ISubscribersRepository subscribersRepository)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _accountRepository = accountRepository;
            _subscribersRepository = subscribersRepository;

        }

        public async Task CreatePost(int authorId, string title, string content)
        {
            var nickname = await _accountRepository.GetUsername(authorId);
            var post = new PostEntity
            {
                AuthorId = authorId,
                AuthorName = nickname,
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _postRepository.CreatePost(post);
        }

        public async Task DeletePost(int postId)
        {
            await _postRepository.DeletePost(postId);
        }

        public async Task<List<PostDTO>> GetAllPosts(int pageNumber, int pageSize)
        {
            var posts = await _postRepository.GetAllPosts(pageNumber, pageSize);
            return _mapper.Map<List<PostDTO>>(posts);
        }

        public async Task UpdatePost(int postId, UpdatePostDTO post)
        {
            var currentPost = await _postRepository.GetPostById(postId);

            if (currentPost != null)
            {
                currentPost.Title = post.Title;
                currentPost.Content = post.Content;
                currentPost.UpdatedAt = DateTime.UtcNow;
                

                await _postRepository.UpdatePost(currentPost);
            }
            else
            {
                throw new PostDoesntExistException();
            }
            
        }

        public async Task<List<PostDTO>> GetSubscribedPosts(int accountId, int pageNumber, int pageSize)
        {
            // Pobieranie listy subskrybowanych kont
            var subscribedAccounts = await _subscribersRepository.GetAccountsSubscriptions(accountId, pageNumber, pageSize);
            var subscribedAccountIds = subscribedAccounts.Select(account => account.AccountId).ToList();

            // Pobieranie postów subskrybowanych użytkowników
            var posts = await _postRepository.GetPostsBySubscribedAccounts(subscribedAccountIds, pageNumber, pageSize);
            return _mapper.Map<List<PostDTO>>(posts);
        }

        public async Task<List<PostDTO>> GetPostsByAccountId(int accountId, int pageNumber, int pageSize)
        {
            var posts = await _postRepository.GetPostsByAccountId(accountId, pageNumber, pageSize);
            return _mapper.Map<List<PostDTO>>(posts);
        }
    }
}
