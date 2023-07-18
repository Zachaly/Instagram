using Instagram.Api.Infrastructure;
using Instagram.Application.Command;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/image")]
    public class ImageController : ControllerBase
    {
        private IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns profile picture of specified user or default one
        /// </summary>
        /// <response code="200">Profile picture</response>
        [HttpGet("profile/{id}")]
        [ProducesResponseType(200)]
        public async Task<FileStreamResult> GetProfilePictureAsync(long id)
        {
            var res = await _mediator.Send(new GetProfilePictureQuery { UserId = id });

            return new FileStreamResult(res, "image/png");
        }

        /// <summary>
        /// Updates profile picture of given user
        /// </summary>
        /// <response code="204">Picture updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPatch("profile")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateProfilePictureAsync([FromForm] UpdateProfilePictureCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns image of post with specified id
        /// </summary>
        /// <response code="200">Post image</response>
        [HttpGet("post/{id}")]
        [ProducesResponseType(200)]
        public async Task<FileStreamResult> GetPostImageAsync(long id)
        {
            var res = await _mediator.Send(new GetPostImageQuery { Id = id });

            return new FileStreamResult(res, "image/png");
        }

        /// <summary>
        /// Returns relation image with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("relation/{id}")]
        public async Task<FileStreamResult> GetRelationImageAsync(long id)
        {
            var res = await _mediator.Send(new GetRelationImageQuery { Id = id });

            return new FileStreamResult(res, "image/png");
        }

        /// <summary>
        /// Returns verification document image with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("account-verificaton/{id}")]
        public async Task<FileStreamResult> GetVerificationImage(long id)
        {
            var res = await _mediator.Send(new GetVerificationDocumentQuery { Id = id });

            return new FileStreamResult(res, "image/jpg");
        }
    }
}
