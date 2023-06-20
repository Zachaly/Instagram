using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.Post.Request
{
    public class GetPostRequest : PagedRequest
    {
        public long? CreatorId { get; set; }
        public long? Id { get; set; }
        [Where(Condition = "[Post].[CreatorId] IN ")]
        public IEnumerable<long>? CreatorIds { get; set; }

        [ConditionalJoin(Condition = "[Post].[Id]=[PostTag].[PostId]", Table = "PostTag")]
        [Where(Condition = "[PostTag].[Tag]=")]
        public string? SearchTag { get; set; }
    }
}
