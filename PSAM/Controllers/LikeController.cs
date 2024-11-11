using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PSAM.DTOs.AccountDTOs;
using PSAM.Models;
using PSAM.Services.IServices;

namespace PSAM.Controllers
{
    public class LikeController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILikeService _likeService;

        public LikeController(IAuthService authService, ILikeService likeService)
        {
            _authService = authService;
            _likeService = likeService;
        }

        [HttpPost("LikePost/{postId}")]
        public async Task<IActionResult> LikePost(int postId)
        {
            try
            {
                var author = _authService.GetUserIdFromToken();
                await _likeService.LikePost(author.Value, postId);
                return Ok(new { message = "Liked successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("UnlikePost/{postId}")]
        public async Task<IActionResult> UnlikePost(int postId)
        {
            await _likeService.UnlikePost(postId);
            return Ok(new { message = "Unliked succesfully" });
        }

        [HttpPost("LikeComment/{commentId}")]
        public async Task<IActionResult> LikeComment(int commentId)
        {
            try
            {
                var author = _authService.GetUserIdFromToken();
                await _likeService.LikeComment(author.Value, commentId);
                return Ok(new { message = "Comment liked successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("UnlikeComment/{commentId}")]
        public async Task<IActionResult> UnlikeComment(int commentId)
        {
            try
            {
                var author = _authService.GetUserIdFromToken();
                await _likeService.UnlikeComment(commentId);
                return Ok(new { message = "Comment unliked successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
