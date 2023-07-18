using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.AccountVerification
{
    [Join(Condition = "[User].[Id]=[AccountVerification].[UserId]", Table = "User")]
    public class AccountVerificationModel : IModel
    {
        public long Id { get; set; }
        [SqlName(Name = "[User].[Name]")]
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Created { get; set; }
        public string DateOfBirth { get; set; }
    }
}
