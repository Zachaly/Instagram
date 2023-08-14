using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(4)]
    public class AddPostImageMigration_07062023 : Migration
    {
        public override void Down()
        {
            Create.Column("FileName").OnTable("Post").AsAnsiString(40).NotNullable();
            Delete.Table("PostImage");
        }

        public override void Up()
        {
            Delete.Column("FileName").FromTable("Post");
            Create.Table("PostImage")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity().NotNullable()
                .WithColumn("File").AsAnsiString(40).NotNullable()
                .WithColumn("PostId").AsInt64().NotNullable();
        }
    }
}
