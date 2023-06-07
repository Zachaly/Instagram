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
    }
}
