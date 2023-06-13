using Instagram.Api.Infrastructure;
using Instagram.Application.Abstraction;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/post-like")]
    public class PostLikeController : ControllerBase
    {
        private readonly IPostLikeService _postLikeService;

        public PostLikeController(IPostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }

        /// <summary>
        /// Returns list of specified post likes
        /// </summary>
        /// <response code="200">List of likes</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<PostLikeModel>>> GetAsync([FromQuery] GetPostLikeRequest request)
        {
            var res = await _postLikeService.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns number of post likes
        /// </summary>
        /// <response code="200">Number of post likes</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetPostLikeRequest request)
        {
            var res = await _postLikeService.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new post like
        /// </summary>
        /// <response code="204">Like added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddPostLikeRequest request)
        {
            var res = await _postLikeService.AddAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes post like with specified post and user id
        /// </summary>
        /// <response code="204">Like removed successfully</response>
        /// <response code="400">Failed to remove specified like</response>
        [HttpDelete("{postId}/{userId}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long postId, long userId)
        {
            var res = await _postLikeService.DeleteAsync(postId, userId);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
