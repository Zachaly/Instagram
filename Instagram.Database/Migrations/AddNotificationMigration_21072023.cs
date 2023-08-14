using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(15)]
    public class AddNotificationMigration_21072023 : Migration
    {
        public override void Down()
        {
            Delete.Table("Notification");
        }

        public override void Up()
        {
            Create.Table("Notification")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("Message").AsString(300).NotNullable()
                .WithColumn("IsRead").AsBoolean().NotNullable()
                .WithColumn("Created").AsInt64().NotNullable();
        }
    }
}
