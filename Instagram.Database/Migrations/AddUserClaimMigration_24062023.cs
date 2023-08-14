using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(9)]
    public class AddUserClaimMigration_24062023 : Migration
    {
        public override void Down()
        {
            Delete.Table("UserClaim");
        }

        public override void Up()
        {
            Create.Table("UserClaim")
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("Value").AsAnsiString(30).NotNullable();
        }
    }
}
