using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application.Auth.Command;
using Instagram.Application.Command;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceProxy _userServiceProxy;
        private readonly IMediator _mediator;

        public UserController(IUserServiceProxy userServiceProxy, IMediator mediator)
        {
            _userServiceProxy = userServiceProxy;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of users with data specified in request
        /// </summary>
        /// <response code="200">List of users</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAsync([FromQuery] GetUserRequest request)
        {
            var res = await _userServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns user with specified id
        /// </summary>
        /// <response code="200">User data</response>
        /// <response code="404">No user with given id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserModel>> GetByIdAsync(long id)
        {
            var res = await _userServiceProxy.GetByIdAsync(id);

            return ResponseModelExtentions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new user to database with data specified in request
        /// </summary>
        /// <response code="201">User successfully created</response>
        /// <response code="400">Invalid user data</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> RegisterAsync(RegisterCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("/api/user/");
        }

        /// <summary>
        /// Authorizes user with data given in request
        /// </summary>
        /// <response code="200">Authorization data</response>
        /// <response code="400">Invalid login data</response>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DataResponseModel<LoginResponse>>> LoginAsync(LoginCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnOkOrBadRequest();
        }

        /// <summary>
        /// Updates specified user with data given in request
        /// </summary>
        /// <response code="204">User updated successfully</response>
        /// <response code="400">Invalid request data</response>
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateUserRequest request)
        {
            var res = await _userServiceProxy.UpdateAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns login data of current user based on JWT token
        /// </summary>
        /// <response code="200">Login data</response>
        /// <response code="400">Failed to get user data</response>
        [HttpGet("current")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DataResponseModel<LoginResponse>>> GetCurrentUserAsync()
        {
            var res = await _mediator.Send(new GetCurrentUserQuery());

            return res.ReturnOkOrBadRequest();
        }

        /// <summary>
        /// Changes password of specified user if old password matches
        /// </summary>
        /// <response code="204">Password changed successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPatch("change-password")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> ChangePasswordAsync(ChangePasswordCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
