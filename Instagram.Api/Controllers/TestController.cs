using Instagram.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/test")]
    public class TestController : ControllerBase
    {
        private ActionResult<string> Answer() => Ok("Works");

        [Authorize(Policy = AuthPolicyNames.Admin)]
        [HttpGet("admin")]
        public ActionResult<string> TestAdminPolicy() => Answer();

        [Authorize(Policy = AuthPolicyNames.Moderator)]
        [HttpGet("moderator")]
        public ActionResult<string> TestModeratorPolicy() => Answer();

        [NotBanned]
        [HttpGet("not-banned")]
        public ActionResult<string> TestNotBannedAttribute() => Answer();
    }
}
