﻿using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Database.Repository
{
    public class UserFollowRepository : KeylessRepositoryBase<UserFollow, UserFollowModel, GetUserFollowRequest>, IUserFollowRepository
    {
        public UserFollowRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "UserFollow";
            DefaultOrderBy = "[UserFollow].[FollowingUserId]";
        }

        public Task DeleteAsync(long followerId, long followedId)
        {
            var param = new { FollowingUserId = followerId, FollowedUserId = followedId };
            var query = _sqlQueryBuilder
                .BuildDelete(Table)
                .Where(param)
                .Build();

            return QueryAsync(query, param);
        }
    }
}
