namespace Instagram.Models.UserClaim.Request
{
    public class GetUserClaimRequest : PagedRequest
    {
        public long? UserId { get; set; }
        public string? Value { get; set; }
    }
}
