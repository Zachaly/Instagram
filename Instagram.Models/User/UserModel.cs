using Instagram.Domain.Enum;

namespace Instagram.Models.User
{
    public class UserModel : IModel
    { 
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public Gender Gender { get; set; }
    }
}
