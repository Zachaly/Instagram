using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(10)]
    public class AddPostReportMigration_26062023 : Migration
    {
        public override void Down()
        {
            Delete.Table("PostReport");
        }

        public override void Up()
        {
            Create.Table("PostReport")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("ReportingUserId").AsInt64().NotNullable()
                .WithColumn("PostId").AsInt64().NotNullable()
                .WithColumn("Reason").AsAnsiString(200).NotNullable()
                .WithColumn("Created").AsInt64().NotNullable()
                .WithColumn("Accepted").AsBoolean().Nullable()
                .WithColumn("Resolved").AsBoolean().NotNullable()
                .WithColumn("ResolveTime").AsInt64().Nullable();
        }
    }
}
