using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.DirectMessage.Request
{
    public class GetDirectMessageRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? SenderId { get; set; }
        public long? ReceiverId { get; set; }
        public long? Created { get; set; }
        public bool? Read { get; set; }
        public string? Content { get; set; }

        [Where(Condition = "[DirectMessage].[SenderId] IN @UserIds AND [DirectMessage].[ReceiverId] IN ")]
        public IEnumerable<long>? UserIds { get; set; }
    }
}
