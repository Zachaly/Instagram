using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.PostComment
{
    public class PostCommentModel : IModel
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long UserId { get; set; }
        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }
        public long Created { get; set; }
    }
}
