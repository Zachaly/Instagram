using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/post-tag")]
    public class PostTagController : ControllerBase
    {
        private readonly IPostTagServiceProxy _postTagServiceProxy;

        public PostTagController(IPostTagServiceProxy postTagServiceProxy)
        {
            _postTagServiceProxy = postTagServiceProxy;
        }

        /// <summary>
        /// Gets list of post tags
        /// </summary>
        /// <response code="200">List of tags</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<PostTagModel>>> GetAsync([FromQuery] GetPostTagRequest request)
        {
            var res = await _postTagServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns count of post tags
        /// </summary>
        /// <response code="200">Number of tags</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetPostTagRequest request)
        {
            var res = await _postTagServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new post tags
        /// </summary>
        /// <response code="204">Tags created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [NotBanned]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddPostTagRequest request)
        {
            var res = await _postTagServiceProxy.AddAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes specified post tag
        /// </summary>
        /// <response code="204">Tag deleted successfully</response>
        /// <response code="400">Failed to delete tag</response>
        [HttpDelete("{postId}/{tag}")]
        [NotBanned]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long postId, string tag)
        {
            var res = await _postTagServiceProxy.DeleteAsync(postId, tag);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
