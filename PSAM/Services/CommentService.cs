using AutoMapper;
using PSAM.DTOs.AccountDTOs;
using PSAM.Entities;
using PSAM.Repositories.IRepositories;
using PSAM.Services.IServices;
using PSAM.Exceptions;
using Microsoft.Identity.Client;

namespace PSAM.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        public readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IAccountRepository _accountRepository;


        public CommentService(IMapper mapper, ICommentRepository commentRepository, IPostRepository postRepository, IAccountRepository accountRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _accountRepository = accountRepository;
        }

        public async Task<List<CommentDTO>> GetAllComments(int pageNumber, int pageSize)
        {
            var comments = await _commentRepository.GetAllComments(pageNumber, pageSize);
            return _mapper.Map<List<CommentDTO>>(comments);
        }

        public async Task<List<CommentDTO>> GetAllCommentsFromPost(int postId, int pageNumber, int pageSize)
        {
            var comments = await _commentRepository.GetAllCommentsFromPost(postId, pageNumber, pageSize);
            return _mapper.Map<List<CommentDTO>>(comments);
        }

        public async Task<List<CommentDTO>> GetAllCommentsFromComment(int commentId, int pageNumber, int pageSize)
        {
            var comments = await _commentRepository.GetAllCommentsFromPost(commentId, pageNumber, pageSize);
            return _mapper.Map<List<CommentDTO>>(comments);
        }

        public async Task CreateComment(int authorId, string text, int? postId = null, int? parentCommentId = null)
        {
            // Sprawdzenie, czy oba identyfikatory są podane lub żaden nie jest podany
            if (postId != null && parentCommentId != null)
            {
                throw new BothIdsProvidedException();
            }
            else if (postId == null && parentCommentId == null)
            {
                throw new NoIdProvidedException();
            }

            // Sprawdzenie istnienia postu lub komentarza w zależności od tego, które ID jest podane
            if (postId.HasValue)
            {
                var post = await _postRepository.GetPostById(postId.Value);
                if (post == null)
                {
                    throw new PostNotFoundException();
                }
            }
            else if (parentCommentId.HasValue)
            {
                var comment = await _commentRepository.GetCommentById(parentCommentId.Value);
                if (comment == null)
                {
                    throw new CommentNotFoundException();
                }
            }
            if (authorId != null)
            {
                string nickname = await _accountRepository.GetUsername(authorId);
                // Tworzenie obiektu komentarza
                var commentEntity = new CommentEntity
                {
                    AuthorId = authorId,
                    AuthorName = nickname,
                    Text = text,
                    PostId = postId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CommentId = parentCommentId // Ustawia parentCommentId, jeśli jest odpowiedzią
                };

                await _commentRepository.CreateComment(commentEntity);
            }
            
        }

        public async Task DeleteComment(int commentId)
        {
            await _commentRepository.DeleteComment(commentId);
        }

        public async Task UpdateComment(int commentId, string text)
        {
            var currentComment = await _commentRepository.GetCommentById(commentId);

            if (currentComment != null)
            {
                currentComment.Text = text;
                currentComment.UpdatedAt = DateTime.UtcNow;

                await _commentRepository.UpdateComment(currentComment);

            }
            else
            {
                throw new CommentNotFoundException();
            }
        }

    }
}
