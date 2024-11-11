using Microsoft.AspNetCore.Mvc;
using PSAM.Exceptions;
using PSAM.Models;
using PSAM.Services;
using PSAM.Services.IServices;

namespace PSAM.Controllers
{
    public class CommentController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICommentService _commentService;

        public CommentController(IAuthService authService, ICommentService commentService)
        {
            _authService = authService;
            _commentService = commentService;
        }

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment(CommentModel commentModel)
        {
            var accountId = _authService.GetUserIdFromToken();

            if (accountId != null)
            {
                try
                {
                    await _commentService.CreateComment(accountId.Value, commentModel.Text, commentModel.PostId, commentModel.ParentCommentId);
                    return Ok();
                }
                catch (BothIdsProvidedException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (NoIdProvidedException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (PostNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (CommentNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }

        [HttpGet("GetAllComments")]
        public async Task<IActionResult> GetAllComments(int pageNumber = 1, int pageSize = 10)
        {
            var comments = await _commentService.GetAllComments(pageNumber, pageSize);
            return Ok(comments);
        }

        [HttpGet("GetAllCommentsFromPost/{postId}")]
        public async Task<IActionResult> GetAllCommentsFromPost(int postId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var comments = await _commentService.GetAllCommentsFromPost(postId, pageNumber, pageSize);
                return Ok(comments);
            }
            catch (PostNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetAllCommentsFromComment/{commentId}")]
        public async Task<IActionResult> GetAllCommentsFromComment(int commentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var comments = await _commentService.GetAllCommentsFromComment(commentId, pageNumber, pageSize);
                return Ok(comments);
            }
            catch (CommentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var accountId = _authService.GetUserIdFromToken();

            if (accountId != null)
            {
                try
                {
                    await _commentService.DeleteComment(commentId);
                    return Ok("Comment deleted successfully.");
                }
                catch (CommentNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }

        [HttpPut("UpdateComment/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] string text)
        {
            var accountId = _authService.GetUserIdFromToken();

            if (accountId != null)
            {
                try
                {
                    await _commentService.UpdateComment(commentId, text);
                    return Ok("Comment updated successfully.");
                }
                catch (CommentNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }
    }
}

