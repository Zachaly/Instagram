﻿using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserStory.Request
{
    public class GetUserStoryRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? Created { get; set; }
        public string? FileName { get; set; }

        [Where(Condition = "[User].[Id] IN ")]
        public IEnumerable<long>? UserIds { get; set; }

        [Where(Condition = "[UserStoryImage].[Created]<=")]
        public long? MaxCreationTime { get; set; }
    }
}
