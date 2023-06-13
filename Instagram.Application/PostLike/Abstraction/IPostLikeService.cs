﻿using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Application.Abstraction
{
    public interface IPostLikeService : IKeylessServiceBase<PostLikeModel, GetPostLikeRequest>
    {
        Task<ResponseModel> AddAsync(AddPostLikeRequest request);
        Task<ResponseModel> DeleteAsync(long postId, long userId);
    }
}
