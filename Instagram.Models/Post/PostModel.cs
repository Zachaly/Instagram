using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.Post
{
    [Join(Table = "[User]", Condition = "[User].[Id]=[Post].[CreatorId]")]
    public class PostModel : IModel
    {
        public long Id { get; set; }
        [SqlName("[User].[Id]")]
        public long CreatorId { get; set; }
        public string Content { get; set; }
        [SqlName("[User].[Nickname]")]
        public string CreatorName { get; set; }
        public long Created { get; set; }
        public IEnumerable<long> ImageIds { get; set; }
    }
}
