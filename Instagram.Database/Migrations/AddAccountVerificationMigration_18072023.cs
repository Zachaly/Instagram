using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(14)]
    public class AddAccountVerificationMigration_18072023 : Migration
    {
        public override void Down()
        {
            Delete.Table("AccountVerification");
            Delete.Column("Verified")
                .FromTable("User");
        }

        public override void Up()
        {
            Create.Column("Verified")
                .OnTable("User")
                .AsBoolean()
                .WithDefaultValue(false)
                .NotNullable();

            Create.Table("AccountVerification")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("Created").AsInt64().NotNullable()
                .WithColumn("FirstName").AsString(100).NotNullable()
                .WithColumn("LastName").AsString(100).NotNullable()
                .WithColumn("DocumentFileName").AsString(40).NotNullable()
                .WithColumn("DateOfBirth").AsString(10).NotNullable();
        }
    }
}
