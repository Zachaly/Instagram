using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application.Command;
using Instagram.Models.AccountVerification;
using Instagram.Models.AccountVerification.Request;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/account-verification")]
    [NotBanned]
    public class AccountVerificationController : ControllerBase
    {
        private readonly IAccountVerificationServiceProxy _accountVerificationServiceProxy;
        private readonly IMediator _mediator;

        public AccountVerificationController(IAccountVerificationServiceProxy accountVerificationServiceProxy, IMediator mediator)
        {
            _accountVerificationServiceProxy = accountVerificationServiceProxy;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of verification requests
        /// </summary>
        /// <response code="200">List of verification requests</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [Authorize(Policy = AuthPolicyNames.Moderator)]
        public async Task<ActionResult<IEnumerable<AccountVerificationModel>>> GetAsync([FromQuery] GetAccountVerificationRequest request)
        {
            var res = await _accountVerificationServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns verification request with specified id
        /// </summary>
        /// <response code="200">Request</response>
        /// <response code="404">No request with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize(Policy = AuthPolicyNames.Moderator)]
        public async Task<ActionResult<AccountVerificationModel>> GetByIdAsync(long id)
        {
            var res = await _accountVerificationServiceProxy.GetByIdAsync(id);

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns count of verification requests
        /// </summary>
        /// <response code="200">Number of requests</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        [Authorize(Policy = AuthPolicyNames.Moderator)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetAccountVerificationRequest request)
        {
            var res = await _accountVerificationServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new verification request with data given in request
        /// </summary>
        /// <response code="201">Request created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync([FromForm] AddAccountVerificationCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("account-verification");
        }
        
        /// <summary>
        /// Resolves verification request with data given in request
        /// </summary>
        /// <response code="204"></response>
        [HttpPut("resolve")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Moderator)]
        public async Task<ActionResult<ResponseModel>> ResolveAsync(ResolveAccountVerificationCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
