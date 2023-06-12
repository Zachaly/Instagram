using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.PostLike
{
    public class PostLikeModel : IModel
    {
        public long UserId { get; set; }
        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }
    }
}
