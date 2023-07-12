using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(11072023)]
    public class AddRelationsMigration_11072023 : Migration
    {
        public override void Down()
        {
            Delete.Table("Relation");
            Delete.Table("RelationImage");
        }

        public override void Up()
        {
            Create.Table("Relation")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("Name").AsString(50).NotNullable();

            Create.Table("RelationImage")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("RelationId").AsInt64().NotNullable()
                .WithColumn("FileName").AsAnsiString(40).NotNullable();
        }
    }
}
