﻿using Instagram.Api.Infrastructure;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/user-follow")]
    public class UserFollowController : ControllerBase
    {
        private readonly IUserFollowService _userFollowService;

        public UserFollowController(IUserFollowService userFollowService)
        {
            _userFollowService = userFollowService;
        }

        /// <summary>
        /// Returns user follows
        /// </summary>
        /// <response code="200">List of follows</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserFollowModel>>> GetAsync([FromQuery] GetUserFollowRequest request)
        {
            var res = await _userFollowService.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns number of user follows
        /// </summary>
        /// <response code="200">Number of follows</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetUserFollowRequest request)
        {
            var res = await _userFollowService.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new user follow
        /// </summary>
        /// <response code="204">Follow added successfully</response>
        /// <response code="400">Failed to add follow</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddUserFollowRequest request)
        {
            var res = await _userFollowService.AddAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes user follow specified with ids
        /// </summary>
        /// <response code="204">Follow deleted successfully</response>
        /// <response code="400">Failed to delete follow</response>
        [HttpDelete("{followerId}/{followedId}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long followerId, long followedId)
        {
            var res = await _userFollowService.DeleteAsync(followerId, followedId);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}