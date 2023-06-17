using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("api/post")]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPostServiceProxy _postServiceProxy;

        public PostController(IMediator mediator, IPostServiceProxy postServiceProxy)
        {
            _mediator = mediator;
            _postServiceProxy = postServiceProxy;
        }

        /// <summary>
        /// Returns list of posts
        /// </summary>
        /// <response code="200">Posts</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<PostModel>>> GetAsync([FromQuery] GetPostRequest request)
        {
            var res = await _postServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns post with specified id
        /// </summary>
        /// <response code="200">Post</response>
        /// <response code="404">No post with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PostModel>> GetByIdAsync(long id)
        {
            var res = await _postServiceProxy.GetByIdAsync(id);

            return ResponseModelExtentions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new post with specified data
        /// </summary>
        /// <response code="201">Post added</response>
        /// <response code="400">Invalid data</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync([FromForm] AddPostCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("/api/post/");
        }

        /// <summary>
        /// Deletes post with specified id
        /// </summary>
        /// <response code="204">Post removed successfully</response>
        /// <response code="400">Failed to remove the post</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long id)
        {
            var res = await _mediator.Send(new DeletePostCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns count of posts specified by request
        /// </summary>
        /// <response code="200">Number of posts</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetPostRequest request)
        {
            var res = await _postServiceProxy.GetCountAsync(request);

            return Ok(res);
        }
    }
}
