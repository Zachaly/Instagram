using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(28072023)]
    public class AddUserStoryImageMigration_28072023 : Migration
    {
        public override void Down()
        {
            Delete.Table("UserStoryImage");
        }

        public override void Up()
        {
            Create.Table("UserStoryImage")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("Created").AsInt64().NotNullable()
                .WithColumn("FileName").AsString(40).NotNullable();
        }
    }
}
