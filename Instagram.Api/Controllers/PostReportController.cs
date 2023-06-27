using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application.Command;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/post-report")]
    [Authorize]
    public class PostReportController : ControllerBase
    {
        private readonly IPostReportServiceProxy _postReportServiceProxy;
        private readonly IMediator _mediator;

        public PostReportController(IPostReportServiceProxy postReportServiceProxy, IMediator mediator)
        {
            _postReportServiceProxy = postReportServiceProxy;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of post reports
        /// </summary>
        /// <response code="200">List of reports</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<PostReportModel>>> GetAsync([FromQuery] GetPostReportRequest request)
        {
            var res = await _postReportServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns post report with specified id
        /// </summary>
        /// <response code="200">Post report model</response>
        /// <response code="404">Post report not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task <ActionResult<PostReportModel>> GetByIdAsync(long id)
        {
            var res = await _postReportServiceProxy.GetByIdAsync(id);

            return ResponseModelExtentions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns number of post reports
        /// </summary>
        /// <response code="200">Number of reports</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetPostReportRequest request)
        {
            var res = await _postReportServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new post report
        /// </summary>
        /// <response code="201">Post report added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddPostReportRequest request)
        {
            var res = await _postReportServiceProxy.AddAsync(request);

            return res.ReturnCreatedOrBadRequest("post-report");
        }

        /// <summary>
        /// Resolves the post report
        /// </summary>
        /// <response code="204">Report resolved</response>
        /// <response code="400">Invalid request</response>
        [HttpPut("resolve")]
        [Authorize(Policy = UserClaimValues.Moderator)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> ResolveAsync(ResolvePostReportCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
