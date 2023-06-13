using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.PostLike
{
    [Join(Table = "User", Condition = "[User].[Id]=[PostLike].[UserId]")]
    public class PostLikeModel : IModel
    {
        public long UserId { get; set; }
        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }
    }
}
