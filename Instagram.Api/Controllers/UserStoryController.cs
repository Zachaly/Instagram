using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application.Command;
using Instagram.Models.Response;
using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/user-story")]
    [Authorize]
    public class UserStoryController : ControllerBase
    {
        private readonly IUserStoryServiceProxy _userStoryServiceProxy;
        private readonly IMediator _mediator;

        public UserStoryController(IUserStoryServiceProxy userStoryServiceProxy, IMediator mediator)
        {
            _userStoryServiceProxy = userStoryServiceProxy;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of user stories
        /// </summary>
        /// <response code="200">List of stories</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserStoryModel>>> GetAsync([FromQuery] GetUserStoryRequest request)
        {
            var res = await _userStoryServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new story images with data given in request
        /// </summary>
        /// <param name="command"></param>
        /// <response code="204">Images added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [NotBanned]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync([FromForm] AddUserStoryImagesCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes story image with specified id
        /// </summary>
        /// <response code="204">Story removed successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpDelete("{imageId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long imageId)
        {
            var res = await _mediator.Send(new DeleteUserStoryImageCommand { Id = imageId });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
