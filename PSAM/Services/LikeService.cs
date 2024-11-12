using AutoMapper;
using PSAM.DTOs.AccountDTOs;
using PSAM.Entities;
using PSAM.Repositories.IRepositories;
using PSAM.Services.IServices;
using System.Runtime.InteropServices;

namespace PSAM.Services
{
    public class LikeService : ILikeService
    {
        private readonly IMapper _mapper;
        private readonly ILikeRepository _likeRepository;
        private readonly IAuthService _authService;

        public LikeService(IMapper mapper, ILikeRepository likeRepository, IAuthService authService)
        {
            _mapper = mapper;
            _likeRepository = likeRepository;
            _authService = authService;
        }

        public async Task LikePost(int authorId, int postId)
        {
            // Sprawdzenie, czy użytkownik już polubił ten post
            var existingLike = await _likeRepository.GetLikedPost(authorId, postId);
            if (existingLike != null)
            {
                throw new InvalidOperationException("Post already liked by this user.");
            }

            // Jeśli lajka nie ma, dodajemy go
            var like = new PostLikeEntity
            {
                AccountId = authorId,
                PostId = postId
            };
            await _likeRepository.LikePost(like, postId);
        }

        public async Task UnlikePost(int postId)
        {
            var authorId = _authService.GetUserIdFromToken();
            if (authorId == null)
            {
                throw new ArgumentException("User not authenticated.");
            }
            var post = await _likeRepository.GetLikedPost(authorId.Value, postId);
            if (post == null)
            {
                throw new ArgumentException("Post aint liked");
            }
            await _likeRepository.UnlikePost(post);
        }
        public async Task LikeComment(int authorId, int commentId)
        {
            // Sprawdzenie, czy użytkownik już polubił ten komentarz
            var existingLike = await _likeRepository.GetLikedComment(authorId, commentId);
            if (existingLike != null)
            {
                throw new InvalidOperationException("Comment already liked by this user.");
            }

            // Jeśli lajka nie ma, dodajemy go
            var like = new CommentLikeEntity
            {
                AccountId = authorId,
                CommentId = commentId
            };
            await _likeRepository.LikeComment(like);
        }

        public async Task UnlikeComment(int commentId)
        {
            var authorId = _authService.GetUserIdFromToken();
            if (authorId == null)
            {
                throw new ArgumentException("User not authenticated.");
            }
            var comment = await _likeRepository.GetLikedComment(authorId.Value, commentId);
            if (comment == null)
            {
                throw new ArgumentException("Comment aint liked");
            }
            await _likeRepository.UnlikeComment(comment);
        }

        public async Task<int> GetPostLikesCount(int postId)
        {
            return await _likeRepository.CountLikesForPost(postId);
        }

        public async Task<int> GetCommentLikesCount(int commentId)
        {
            return await _likeRepository.CountLikesForComment(commentId);
        }

    }
}
