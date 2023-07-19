namespace Instagram.Domain.Entity
{
    public class AccountVerification : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long Created { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentFileName { get; set; }
        public string DateOfBirth { get; set; }
    }
}
