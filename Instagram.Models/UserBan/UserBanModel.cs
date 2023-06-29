using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserBan
{
    [Join(Table = "User", Condition = "[User].[Id]=[UserBan].[UserId]")]
    public class UserBanModel : IModel
    {
        public long Id { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public long UserId { get; set; }
        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }
    }
}
