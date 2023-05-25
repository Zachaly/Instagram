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
                .RuleFor(u => u.Nickname, f => f.Random.Word())
                .RuleFor(u => u.PasswordHash, _ => "Hash")
                .RuleFor(u => u.Bio, _ => "")
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .Generate(count); 
    }
}
