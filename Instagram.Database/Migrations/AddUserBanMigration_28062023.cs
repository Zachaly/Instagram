using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(28062023)]
    public class AddUserBanMigration_28062023 : Migration
    {
        public override void Down()
        {
            Delete.Table("UserBan");
        }

        public override void Up()
        {
            Create.Table("UserBan")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("EndDate").AsInt64().NotNullable()
                .WithColumn("StartDate").AsInt64().NotNullable();
        }
    }
}
