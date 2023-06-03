using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(02062023)]
    public class AddPostsMigration_02062023 : Migration
    {
        public override void Down()
        {
            Delete.Table("Post");
        }

        public override void Up()
        {
            Create.Table("Post")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("Content").AsString(200).NotNullable()
                .WithColumn("CreatorId").AsInt64().NotNullable()
                .WithColumn("FileName").AsAnsiString(40).NotNullable()
                .WithColumn("Created").AsInt64().NotNullable();
        }
    }
}
