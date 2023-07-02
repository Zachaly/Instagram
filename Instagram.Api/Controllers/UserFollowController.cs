using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/user-follow")]
    public class UserFollowController : ControllerBase
    {
        private readonly IUserFollowServiceProxy _userFollowServiceProxy;

        public UserFollowController(IUserFollowServiceProxy userFollowServiceProxy)
        {
            _userFollowServiceProxy = userFollowServiceProxy;
        }

        /// <summary>
        /// Returns user follows
        /// </summary>
        /// <response code="200">List of follows</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserFollowModel>>> GetAsync([FromQuery] GetUserFollowRequest request)
        {
            var res = await _userFollowServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns number of user follows
        /// </summary>
        /// <response code="200">Number of follows</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetUserFollowRequest request)
        {
            var res = await _userFollowServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new user follow
        /// </summary>
        /// <response code="204">Follow added successfully</response>
        /// <response code="400">Failed to add follow</response>
        [HttpPost]
        [NotBanned]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddUserFollowRequest request)
        {
            var res = await _userFollowServiceProxy.AddAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes user follow specified with ids
        /// </summary>
        /// <response code="204">Follow deleted successfully</response>
        /// <response code="400">Failed to delete follow</response>
        [HttpDelete("{followerId}/{followedId}")]
        [NotBanned]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long followerId, long followedId)
        {
            var res = await _userFollowServiceProxy.DeleteAsync(followerId, followedId);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
