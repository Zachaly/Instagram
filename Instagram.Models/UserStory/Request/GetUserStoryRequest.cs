using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserStory.Request
{
    public class GetUserStoryRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? Created { get; set; }
        public string? FileName { get; set; }

        [Where(Condition = "[UserStoryImage].[UserId] IN ")]
        public IEnumerable<long>? UserIds { get; set; }
    }
}
