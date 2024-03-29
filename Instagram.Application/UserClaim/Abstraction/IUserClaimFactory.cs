﻿using Instagram.Domain.Entity;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserClaimFactory : IEntityFactory<UserClaim, AddUserClaimRequest>
    {
    }
}
