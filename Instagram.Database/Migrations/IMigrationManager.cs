namespace Instagram.Database.Migrations
{
    public interface IMigrationManager
    {
        void CreateDatabase();
        void MigrateDatabase();
    }
}
