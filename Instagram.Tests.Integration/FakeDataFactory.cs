using Bogus;
using Instagram.Domain.Entity;
using Instagram.Domain.Enum;

namespace Instagram.Tests.Integration
{
    public static class FakeDataFactory
    {
        public static List<User> GenerateUsers(int count)
            => new Faker<User>()
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Nickname, f => f.Random.AlphaNumeric(10))
                .RuleFor(u => u.PasswordHash, _ => "Hash")
                .RuleFor(u => u.Bio, _ => "")
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .Generate(count);

        public static List<Post> GeneratePosts(int count, long userId)
            => new Faker<Post>()
                .RuleFor(p => p.CreatorId, _ => userId)
                .RuleFor(p => p.Content, f => f.Random.AlphaNumeric(30))
                .RuleFor(p => p.Created, f => f.Random.Long(0, DateTimeOffset.UtcNow.ToUnixTimeSeconds()))
                .Generate(count);

        public static List<PostImage> GeneratePostImages(long postId, int count)
            => new Faker<PostImage>()
                .RuleFor(i => i.PostId, _ => postId)
                .RuleFor(i => i.File, f => f.Random.AlphaNumeric(40))
                .Generate(count);

        public static List<UserFollow> GenerateFollows(int count)
            => new Faker<UserFollow>()
                .RuleFor(f => f.FollowedUserId, f => f.Random.Int(0, 20))
                .RuleFor(f => f.FollowingUserId, f => f.Random.Int(0, 20))
                .Generate(count);

        public static List<PostLike> GeneratePostLikes(IEnumerable<long> postIds, IEnumerable<long> userIds)
        {
            var likes = new List<PostLike>();

            foreach(var postId in postIds)
            {
                likes.AddRange(userIds.Select(userId => new PostLike { PostId = postId, UserId = userId }));
            }

            return likes;
        }

        public static List<PostComment> GeneratePostComments(long userId, int count)
            => new Faker<PostComment>()
                .RuleFor(p => p.UserId, _ => userId)
                .RuleFor(p => p.Content, f => f.Random.AlphaNumeric(30))
                .RuleFor(p => p.Created, f => f.Random.Long(0, DateTimeOffset.UtcNow.ToUnixTimeSeconds()))
                .RuleFor(p => p.PostId, f => f.Random.Long(1, 5))
                .Generate(count);

        public static List<PostTag> GeneratePostTags(long postId, int count)
            => new Faker<PostTag>()
                .RuleFor(t => t.PostId, _ => postId)
                .RuleFor(t => t.Tag, f => f.Random.AlphaNumeric(15))
                .Generate(count);

        public static List<UserClaim> GenerateUserClaims(long userId, int count)
            => new Faker<UserClaim>()
                .RuleFor(c => c.UserId, _ => userId)
                .RuleFor(c => c.Value, f => f.Random.AlphaNumeric(10))
                .Generate(count);

        public static List<PostReport> GeneratePostReports(long postId, IEnumerable<long> userIds)
            => new Faker<PostReport>()
                .RuleFor(r => r.Created, f => f.Random.Long(0, DateTimeOffset.UtcNow.ToUnixTimeSeconds()))
                .RuleFor(r => r.Resolved, _ => false)
                .RuleFor(r => r.Accepted, _ => null)
                .RuleFor(r => r.ResolveTime, _ => null)
                .RuleFor(r => r.ReportingUserId, f => f.PickRandom(userIds))
                .RuleFor(r => r.PostId, f => postId)
                .RuleFor(r => r.Reason, f => f.Random.AlphaNumeric(20))
                .Generate(userIds.Count());

        public static List<UserBan> GenerateUserBans(IEnumerable<long> userIds)
            => userIds
                .Select(id => new UserBan 
                { 
                    UserId = id,
                    EndDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    StartDate = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                }).ToList();

        public static List<Relation> GenerateRelations(long userId, int count)
            => new Faker<Relation>()
                .RuleFor(r => r.UserId, _ => userId)
                .RuleFor(r => r.Name, f => f.Random.AlphaNumeric(20))
                .Generate(count);

        public static List<RelationImage> GenerateRelationImages(long relationId, int count)
            => new Faker<RelationImage>()
                .RuleFor(i => i.RelationId, _ => relationId)
                .RuleFor(i => i.FileName, f => f.Random.AlphaNumeric(15))
                .Generate(count);

        public static List<DirectMessage> GenerateDirectMessages(long senderId, long receiverId, int count)
            => new Faker<DirectMessage>()
                .RuleFor(m => m.SenderId, _ => senderId)
                .RuleFor(m => m.ReceiverId, _ => receiverId)
                .RuleFor(m => m.Read, _ => false)
                .RuleFor(m => m.Content, f => f.Random.AlphaNumeric(20))
                .RuleFor(m => m.Created, f => f.Random.Number(int.MaxValue))
                .Generate(count);
    }
}
