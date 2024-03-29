﻿using FluentMigrator;

namespace Instagram.Database.Migrations
{
    [Migration(2)]
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
                .AsAnsiString(40) // GUID string length + extension
                .Nullable();
        }
    }
}
