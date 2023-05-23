using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(22052023)]
    public class AddUsersMigration_22052023 : Migration
    {
        public override void Down()
        {
            Delete.Table("User");
        }

        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("Nickname").AsString(25).NotNullable()
                .WithColumn("Email").AsString(70).NotNullable()
                .WithColumn("Gender").AsInt16().NotNullable()
                .WithColumn("PasswordHash").AsString()
                .WithColumn("Bio").AsString(200);
        }
    }
}
