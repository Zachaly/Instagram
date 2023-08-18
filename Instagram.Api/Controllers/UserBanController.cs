using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.Response;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/user-ban")]
    [Authorize]
    public class UserBanController : ControllerBase
    {
        private readonly IUserBanServiceProxy _userBanServiceProxy;

        public UserBanController(IUserBanServiceProxy userBanServiceProxy)
        {
            _userBanServiceProxy = userBanServiceProxy;
        }

        /// <summary>
        /// Returns list of user bans
        /// </summary>
        /// <response code="200">List of bans</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserBanModel>>> GetAsync([FromQuery] GetUserBanRequest request)
        {
            var res = await _userBanServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns user ban with specified id
        /// </summary>
        /// <response code="200">User ban model</response>
        /// <response code="404">No user ban with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserBanModel>> GetByIdAsync(long id)
        {
            var res = await _userBanServiceProxy.GetByIdAsync(id);

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns number of user bans
        /// </summary>
        /// <response code="200">Number of bans</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetUserBanRequest request)
        {
            var res = await _userBanServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new user ban
        /// </summary>
        /// <response code="204">User ban successfuly created</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [Authorize(AuthPolicyNames.Moderator)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddUserBanRequest request)
        {
            var res = await _userBanServiceProxy.AddAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes user ban with specified id
        /// </summary>
        /// <response code="204">Ban deleted successfully</response>
        /// <response code="400">Failed to delete ban</response>
        [HttpDelete("{id}")]
        [Authorize(AuthPolicyNames.Moderator)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long id)
        {
            var res = await _userBanServiceProxy.DeleteAsync(id);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
