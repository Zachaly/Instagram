﻿using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.VerificationRequest;
using Instagram.Models.VerificationRequest.Request;

namespace Instagram.Database.Repository
{
    public interface IAccountVerificationRepository : IRepositoryBase<AccountVerification, AccountVerificationModel, GetAccountVerificationRequest>
    {
    }
}
