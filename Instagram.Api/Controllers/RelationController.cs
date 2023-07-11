using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application.Command;
using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/relation")]
    public class RelationController : ControllerBase
    {
        private readonly IRelationServiceProxy _relationServiceProxy;
        private readonly IMediator _mediator;

        public RelationController(IRelationServiceProxy relationServiceProxy, IMediator mediator)
        {
            _relationServiceProxy = relationServiceProxy;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of relations
        /// </summary>
        /// <response code="200">List of relations</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<RelationModel>>> GetAsync([FromQuery] GetRelationRequest request)
        {
            var res = await _relationServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns relation with specified id
        /// </summary>
        /// <response code="200">Relation</response>
        /// <response code="404">Relation not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RelationModel>> GetByIdAsync(long id)
        {
            var res = await _relationServiceProxy.GetByIdAsync(id);

            return ResponseModelExtentions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns number of relations
        /// </summary>
        /// <response code="200">Number of relations</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetRelationRequest request)
        {
            var res = await _relationServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new relation with data specified in request
        /// </summary>
        /// <response code="204">Relation added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [NotBanned]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync([FromForm] AddRelationCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes relation with specified id
        /// </summary>
        /// <response code="204">Relation deleted successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpDelete("{id}")]
        [NotBanned]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long id)
        {
            var res = await _mediator.Send(new DeleteRelationCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
