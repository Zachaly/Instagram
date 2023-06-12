﻿using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class PostLikeRepositoryTests : RepositoryTest
    {
        private readonly PostLikeRepository _repository;

        public PostLikeRepositoryTests() : base()
        {
            _repository = new PostLikeRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProperRows()
        {
            const long PostIdToDelete = 2;
            const long UserIdToDelete = 3;

            var postIds = new long[] { 1, PostIdToDelete, 3, 4, 5 };
            var userIds = new long[] { 1, 2, UserIdToDelete, 4, 5 };

            foreach(var like in FakeDataFactory.GeneratePostLikes(postIds, userIds))
            {
                Insert("PostLike", like);
            }

            await _repository.DeleteAsync(PostIdToDelete, UserIdToDelete);

            var likes = GetFromDatabase<PostLike>("SELECT * FROM PostLike");

            Assert.DoesNotContain(likes, x => x.UserId == UserIdToDelete && x.PostId == PostIdToDelete);
        }
    }
}
