using Microsoft.AspNetCore.Authorization;

namespace Instagram.Api.Authorization
{
    public class NotBannedAttribute : AuthorizeAttribute
    {
        public NotBannedAttribute()
        {
            Policy = AuthPolicyNames.NotBanned;
        }
    }
}
