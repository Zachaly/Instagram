using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserBlock
{
    [Join(Condition = "[User].[Id]=[UserBlock].[BlockedUserId]")]
    public class UserBlockModel : IModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }
    }
}
