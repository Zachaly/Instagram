using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(6)]
    public class AddPostLikeMigration_12062023 : Migration
    {
        public override void Down()
        {
            Delete.Table("PostLike");
        }

        public override void Up()
        {
            Create.Table("PostLike")
                .WithColumn("PostId").AsInt64().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable();
        }
    }
}
