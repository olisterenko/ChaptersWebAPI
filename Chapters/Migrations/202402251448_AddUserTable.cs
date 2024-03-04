using FluentMigrator;

namespace Chapters;

[TimestampedMigration(2024, 2, 25, 14, 48)]
public class AddUserTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("User")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Username").AsString(255).NotNullable().Unique()
            .WithColumn("PasswordHash").AsString(511).NotNullable();
    }
}