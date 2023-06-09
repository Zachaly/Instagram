﻿using Instagram.Models.Response;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserBanService : IServiceBase<UserBanModel, GetUserBanRequest>
    {
        Task<ResponseModel> AddAsync(AddUserBanRequest request);
        Task<ResponseModel> DeleteAsync(long id);
    }
}
