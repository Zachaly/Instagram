using Instagram.Api.Infrastructure;
using Instagram.Application.Command;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/relation-image")]
    public class RelationImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RelationImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Adds new relation image with data given in request
        /// </summary>
        /// <response code="204">Image added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync([FromForm] AddRelationImageCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes relation image with specified id
        /// </summary>
        /// <response code="204">Image removed successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long id)
        {
            var res = await _mediator.Send(new DeleteRelationImageCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
