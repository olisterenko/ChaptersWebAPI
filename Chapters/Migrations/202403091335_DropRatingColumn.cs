using FluentMigrator;

namespace Chapters;

[TimestampedMigration(2024, 3, 9, 13, 34)]
public class DropRatingColumn : ForwardOnlyMigration {
    public override void Up()
    {
        Delete.Column("Rating").FromTable("Book");
    }
}