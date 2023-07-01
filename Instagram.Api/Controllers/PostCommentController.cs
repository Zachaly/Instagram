using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/post-comment")]
    public class PostCommentController : ControllerBase
    {
        private readonly IPostCommentServiceProxy _postCommentServiceProxy;

        public PostCommentController(IPostCommentServiceProxy postCommentServiceProxy)
        {
            _postCommentServiceProxy = postCommentServiceProxy;
        }

        /// <summary>
        /// Returns list of post comments
        /// </summary>
        /// <response code="200">List of comments</response>
        [HttpGet]
        [ProducesResponseType(200)] 
        public async Task<ActionResult<IEnumerable<PostCommentModel>>> GetAsync([FromQuery] GetPostCommentRequest request)
        {
            var res = await _postCommentServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns post comment with specified id
        /// </summary>
        /// <response code="200">Comment</response>
        /// <response code="404">Comment with specified id does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PostCommentModel>> GetByIdAsync(long id)
        {
            var res = await _postCommentServiceProxy.GetByIdAsync(id);

            return ResponseModelExtentions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns number of post comments
        /// </summary>
        /// <response code="200">Count</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetPostCommentRequest request)
        {
            var res = await _postCommentServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new post comment
        /// </summary>
        /// <param name="request"></param>
        /// <response code="201">Comment added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [NotBanned]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddPostCommentRequest request)
        {
            var res = await _postCommentServiceProxy.AddAsync(request);

            return res.ReturnCreatedOrBadRequest("/api/post-comment");
        }

        
    }
}
