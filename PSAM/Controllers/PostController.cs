using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PSAM.DTOs.AccountDTOs;
using PSAM.Entities;
using PSAM.Models;
using PSAM.Services.IServices;

namespace PSAM.Controllers
{
    public class PostController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPostService _postService;

        public PostController(IPostService postService, IAuthService authService)
        {
            _authService = authService;
            _postService = postService;
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost(PostModel postModel)
        {
            var accountId = _authService.GetUserIdFromToken();

            if (accountId != null)
            {
                await _postService.CreatePost(accountId.Value, postModel.Title, postModel.Content);
                return Ok();
            }
            else
            {
               return Unauthorized("User not authenticated.");
            }
        }

        [HttpDelete("DeletePost")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            await _postService.DeletePost(postId);
            return Ok();
        }

        [HttpGet("GetPosts")]
        public async Task<IActionResult> GetPosts(int pageNumber = 1, int pageSize = 10)
        {
            var posts = await _postService.GetAllPosts(pageNumber, pageSize); 
            return Ok(posts);
        }

        [HttpPatch("UpdatePost")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostDTO postDTO)
        {
            try
            {
                await _postService.UpdatePost(postId, postDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the post.", error = ex.Message });
            }
        }

        [HttpGet("SubscribedPosts/{accountId}")]
        public async Task<IActionResult> GetSubscribedPosts(int accountId, int pageNumber = 1, int pageSize = 10)
        {
            var posts = await _postService.GetSubscribedPosts(accountId, pageNumber, pageSize);
            return Ok(posts);
        }

        [HttpGet("GetPostsByAccountId/{accountId}")]
        public async Task<IActionResult> GetPostsByAccountId(int accountId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var posts = await _postService.GetPostsByAccountId(accountId, pageNumber, pageSize);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving posts.", error = ex.Message });
            }
        }

    }
}
