using System.Data;

namespace Instagram.Database.Factory
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
        IDbConnection CreateMasterConnection();
    }
}
