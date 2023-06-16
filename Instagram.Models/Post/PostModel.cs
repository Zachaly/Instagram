using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.Post
{
    [Join(Table = "User", Condition = "[User].[Id]=[Post].[CreatorId]")]
    [Join(Table = "PostImage", Condition = "t.[Id]=[PostImage].[PostId]", OutsideJoin = true)]
    [Join(Table = "PostLike", Condition = "t.[Id]=[PostLike].[PostId]", OutsideJoin = true)]
    [Join(Table = "PostComment", Condition = "t.[Id]=[PostComment].[PostId]", OutsideJoin = true)]
    [GroupBy]
    public class PostModel : IModel
    {
        public long Id { get; set; }
        public long CreatorId { get; set; }
        public string Content { get; set; }
        [SqlName("[User].[Nickname]")]
        public string CreatorName { get; set; }
        public long Created { get; set; }
        [SqlName("[PostImage].[Id]", OuterQuery = true)]
        public IEnumerable<long> ImageIds { get; set; }
        [SqlName("Count([PostLike].[UserId]) as LikeCount", OuterQuery = true)]
        [NotGrouped]
        public int LikeCount { get; set; }

        [SqlName("Count([PostComment].[Id]) as CommentCount", OuterQuery = true)]
        [NotGrouped]
        public int CommentCount { get; set; }
    }
}
