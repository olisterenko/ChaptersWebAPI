using FluentMigrator;

namespace Chapters;

[TimestampedMigration(2024, 3, 9, 17, 50)]
public class AddUserActivityTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("UserActivity")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("User", "Id")
            .WithColumn("Text").AsString(10000).NotNullable()
            .WithColumn("CreatedAt").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}