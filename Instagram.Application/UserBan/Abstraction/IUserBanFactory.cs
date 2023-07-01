﻿using Instagram.Domain.Entity;
using Instagram.Models.UserBan.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserBanFactory
    {
        UserBan Create(AddUserBanRequest request);
    }
}
