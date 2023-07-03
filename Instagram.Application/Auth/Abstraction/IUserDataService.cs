namespace Instagram.Application.Auth.Abstraction
{
    public interface IUserDataService
    {
        Task<long?> GetCurrentUserId();
    }
}
