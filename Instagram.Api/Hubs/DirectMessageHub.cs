using Instagram.Api.Authorization;
using Instagram.Models.DirectMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Instagram.Api.Hubs
{
    public interface IDirectMessageClient
    {
        Task MessageReceived(DirectMessageModel directMessageModel);
        Task MessageRead(long messageId, bool isRead);
    }

    [Authorize(AuthenticationSchemes = AuthPolicyNames.WebSocketScheme)]
    public class DirectMessageHub : Hub<IDirectMessageClient>
    {
    }
}
