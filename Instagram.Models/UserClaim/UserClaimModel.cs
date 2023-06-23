using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserClaim
{
    [Join(Table = "User", Condition = "[User].[Id]=[UserClaim].[UserId]")]
    public class UserClaimModel : IModel
    {
        public long UserId { get; set; }
        public string Value { get; set; }
        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }
    }
}
