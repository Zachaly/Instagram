using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(7)]
    public class AddPostCommentMigration_16062023 : Migration
    {
        public override void Down()
        {
            Delete.Table("PostComment");
        }

        public override void Up()
        {
            Create.Table("PostComment")
                .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("PostId").AsInt64().NotNullable()
                .WithColumn("Content").AsString(200).NotNullable()
                .WithColumn("Created").AsInt64().NotNullable();
        }
    }
}
