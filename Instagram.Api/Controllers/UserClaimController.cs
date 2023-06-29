using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.Response;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/user-claim")]
    [Authorize(Policy = AuthPolicyNames.Admin)]
    public class UserClaimController : ControllerBase
    {
        private readonly IUserClaimServiceProxy _userClaimServiceProxy;

        public UserClaimController(IUserClaimServiceProxy userClaimServiceProxy)
        {
            _userClaimServiceProxy = userClaimServiceProxy;
        }

        /// <summary>
        /// Returns list of user claims
        /// </summary>
        /// <response code="200">List of claims</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserClaimModel>>> GetAsync([FromQuery] GetUserClaimRequest request)
        {
            var res = await _userClaimServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns number of user claims
        /// </summary>
        /// <response code="200">Number of claims</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetUserClaimRequest request)
        {
            var res = await _userClaimServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new user claim
        /// </summary>
        /// <response code="204">Claim added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddUserClaimRequest request)
        {
            var res = await _userClaimServiceProxy.AddAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes user claim
        /// </summary>
        /// <response code="204">Claim deleted successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpDelete("{userId}/{value}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long userId, string value)
        {
            var res = await _userClaimServiceProxy.DeleteAsync(userId, value);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
