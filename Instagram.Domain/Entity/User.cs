using Instagram.Domain.Enum;

namespace Instagram.Domain.Entity
{
    public class User : IEntity
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string PasswordHash { get; set; }
        public Gender Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public bool Verified { get; set; }
    }
}
