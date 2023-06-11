using Instagram.Domain.Enum;
using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.User.Request
{
    public class GetUserRequest : PagedRequest
    {
        public long? Id { get; set; }
        public string? Email { get; set; }
        public string? Nickname { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? PasswordHash { get; set; }
        public Gender? Gender { get; set; }
        [Where(Condition = "[User].[Id] IN ")]
        public IEnumerable<long>? UserIds { get; set; }
        [Where(Condition = "[User].[Id] NOT IN ")]
        public IEnumerable<int>? SkipIds { get; set; }
    }
}
