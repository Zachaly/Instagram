using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(09062023)]
    public class AddUserFollowMigration_09062023 : Migration
    {
        public override void Down()
        {
            Delete.Table("UserFollow");
        }

        public override void Up()
        {
            Create.Table("UserFollow")
                .WithColumn("FollowingUserId").AsInt64().NotNullable()
                .WithColumn("FollowedUserId").AsInt64().NotNullable();
        }
    }
}
