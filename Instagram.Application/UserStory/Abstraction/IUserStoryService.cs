﻿using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserStoryService
    {
        Task<IEnumerable<UserStoryModel>> GetAsync(GetUserStoryRequest request);
    }
}
