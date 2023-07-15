using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/direct-message")]
    [Authorize]
    public class DirectMessageController : ControllerBase
    {
        private readonly IDirectMessageServiceProxy _directMessageServiceProxy;

        public DirectMessageController(IDirectMessageServiceProxy directMessageServiceProxy)
        {
            _directMessageServiceProxy = directMessageServiceProxy;
        }

        /// <summary>
        /// Returns list of direct messages
        /// </summary>
        /// <response code="200">List of messages</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<DirectMessageModel>>> GetAsync([FromQuery] GetDirectMessageRequest request)
        {
            var res = await _directMessageServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns direct message with specified id
        /// </summary>
        /// <response code="200">Message</response>
        /// <response code="404">No message with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DirectMessageModel>> GetByIdAsync(long id)
        {
            var res = await _directMessageServiceProxy.GetByIdAsync(id);

            return ResponseModelExtentions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns number of direct messages
        /// </summary>
        /// <response code="200">Number of messages</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetDirectMessageRequest request)
        {
            var res = await _directMessageServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new direct message with data given in request
        /// </summary>
        /// <response code="201">Message added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [NotBanned]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddDirectMessageRequest request)
        {
            var res = await _directMessageServiceProxy.AddAsync(request);

            return res.ReturnCreatedOrBadRequest("direct-message");
        }

        /// <summary>
        /// Updates direct message with data given in request
        /// </summary>
        /// <response code="204">Message updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PatchAsync(UpdateDirectMessageRequest request)
        {
            var res = await _directMessageServiceProxy.UpdateAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes message with specified id
        /// </summary>
        /// <response code="204">Message deleted successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long id)
        {
            var res = await _directMessageServiceProxy.DeleteByIdAsync(id);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
