using Instagram.Api.Infrastructure;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Controllers
{
    [Route("/api/notification")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServiceProxy _notificationServiceProxy;

        public NotificationController(INotificationServiceProxy notificationServiceProxy)
        {
            _notificationServiceProxy = notificationServiceProxy;
        }

        /// <summary>
        /// Returns list of notifications
        /// </summary>
        /// <response code="200">Notification list</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<NotificationModel>>> GetAsync([FromQuery] GetNotificationRequest request)
        {
            var res = await _notificationServiceProxy.GetAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Returns notification with specified id
        /// </summary>
        /// <response code="200">Notification</response>
        /// <response code="404">No notification with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NotificationModel>> GetByIdAsync(long id)
        {
            var res = await _notificationServiceProxy.GetByIdAsync(id);

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns number of notifications specified by request
        /// </summary>
        /// <response code="200">Number of notifications</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetNotificationRequest request)
        {
            var res = await _notificationServiceProxy.GetCountAsync(request);

            return Ok(res);
        }

        /// <summary>
        /// Adds new notification with data specified by request
        /// </summary>
        /// <response code="201">Notification added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddNotificationRequest request)
        {
            var res = await _notificationServiceProxy.AddAsync(request);

            return res.ReturnCreatedOrBadRequest("notification");
        }

        /// <summary>
        /// Deletes notification with specified id
        /// </summary>
        /// <response code="204">Notification deleted successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _notificationServiceProxy.DeleteByIdAsync(id);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Updates notification with specified id with data given in request
        /// </summary>
        /// <response code="204">Notification updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateNotificationRequest request)
        {
            var res = await _notificationServiceProxy.UpdateAsync(request);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
