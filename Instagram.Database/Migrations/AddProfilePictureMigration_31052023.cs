using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(31052023)]
    public class AddProfilePictureMigration_31052023 : Migration
    {
        public override void Down()
        {
            Delete.Column("ProfilePicture").FromTable("User");
        }

        public override void Up()
        {
            Alter.Table("User")
                .AddColumn("ProfilePicture")
                .AsAnsiString(36) // GUID string length
                .Nullable();
        }
    }
}
