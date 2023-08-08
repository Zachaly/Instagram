using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.Response;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/user-block")]
    [Authorize]
    public class UserBlockControllerTests : ControllerBase
    {
        private readonly IUserBlockServiceProxy _userBlockServiceProxy;

        public UserBlockControllerTests(IUserBlockServiceProxy userBlockServiceProxy)
        {
            _userBlockServiceProxy = userBlockServiceProxy;
        }

        /// <summary>
        /// Returns list of user block specified by request
        /// </summary>
        /// <response code="200">List of blocks</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserBlockModel>>> GetAsync([FromQuery] GetUserBlockRequest request)
        {
            var res = await _userBlockServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns user block with specified id
        /// </summary>
        /// <response code="200">User block</response>
        /// <response code="404">No user block with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserBlockModel>> GetByIdAsync(long id)
        {
            var res = await _userBlockServiceProxy.GetByIdAsync(id);

            return ResponseModelExtentions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns number of user blocks specified by request
        /// </summary>
        /// <response code="200">Number of blocks</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetUserBlockRequest request)
        {
            var res = await _userBlockServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new user block with data specified in request
        /// </summary>
        /// <response code="201">User block added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddUserBlockRequest request)
        {
            var res = await _userBlockServiceProxy.AddAsync(request);

            return res.ReturnCreatedOrBadRequest("user-block");
        }

        /// <summary>
        /// Deletes user block with specified id
        /// </summary>
        /// <response code="204">User block deleted successfully</response>
        /// <response code="400">Failed to delete user block</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _userBlockServiceProxy.DeleteByIdAsync(id);

            return res.ReturnNoContentOrBadRequest();
        }

    }
}
