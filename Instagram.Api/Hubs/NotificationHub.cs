using Instagram.Api.Authorization;
using Instagram.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Instagram.Api.Hubs
{
    public interface NotificationClient
    {
        Task NotificationReceived(NotificationModel notification);
    }

    [Authorize(AuthenticationSchemes = AuthPolicyNames.WebSocketScheme)]
    public class NotificationHub : Hub<NotificationClient>
    {
    }
}
