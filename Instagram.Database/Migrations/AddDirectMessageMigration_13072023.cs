using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(13072023)]
    public class AddDirectMessageMigration_13072023 : Migration
    {
        public override void Down()
        {
            Delete.Table("DirectMessage");
        }

        public override void Up()
        {
            Create.Table("DirectMessage")
                .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                .WithColumn("Content").AsString(300).NotNullable()
                .WithColumn("SenderId").AsInt64().NotNullable()
                .WithColumn("ReceiverId").AsInt64().NotNullable()
                .WithColumn("Read").AsBoolean().NotNullable()
                .WithColumn("Created").AsInt64().NotNullable();
        }
    }
}
