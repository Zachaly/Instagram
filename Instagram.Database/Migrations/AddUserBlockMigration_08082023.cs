using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(8082923)]
    public class AddUserBlockMigration_08082023 : Migration
    {
        public override void Down()
        {
            Delete.Table("UserBlock");
        }

        public override void Up()
        {
            Create.Table("UserBlock")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("BlockingUserId").AsInt64().NotNullable()
                .WithColumn("BlockedUserId").AsInt64().NotNullable();
        }
    }
}
