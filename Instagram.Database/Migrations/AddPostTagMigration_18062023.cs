using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(18062023)]
    public class AddPostTagMigration_18062023 : Migration
    {
        public override void Down()
        {
            Delete.Table("PostTag");
        }

        public override void Up()
        {
            Create.Table("PostTag")
                .WithColumn("PostId").AsInt64().NotNullable()
                .WithColumn("Tag").AsAnsiString(30).NotNullable();
        }
    }
}
