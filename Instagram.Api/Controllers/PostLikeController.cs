using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/post-like")]
    public class PostLikeController : ControllerBase
    {
        private readonly IPostLikeServiceProxy _postLikeServiceProxy;

        public PostLikeController(IPostLikeServiceProxy postLikeServiceProxy)
        {
            _postLikeServiceProxy = postLikeServiceProxy;
        }

        /// <summary>
        /// Returns list of specified post likes
        /// </summary>
        /// <response code="200">List of likes</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<PostLikeModel>>> GetAsync([FromQuery] GetPostLikeRequest request)
        {
            var res = await _postLikeServiceProxy.GetAsync(request);

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
            var res = await _postLikeServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new post like
        /// </summary>
        /// <response code="204">Like added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [NotBanned]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddPostLikeRequest request)
        {
            var res = await _postLikeServiceProxy.AddAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes post like with specified post and user id
        /// </summary>
        /// <response code="204">Like removed successfully</response>
        /// <response code="400">Failed to remove specified like</response>
        [HttpDelete("{postId}/{userId}")]
        [NotBanned]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long postId, long userId)
        {
            var res = await _postLikeServiceProxy.DeleteAsync(postId, userId);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
