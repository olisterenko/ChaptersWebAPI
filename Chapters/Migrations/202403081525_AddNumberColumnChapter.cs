using Chapters.Domain.Enums;
using FluentMigrator;

namespace Chapters;

[TimestampedMigration(2024, 3, 8, 15, 25)]
public class AddNumberColumnChapter : ForwardOnlyMigration {
    public override void Up()
    {
        Alter.Table("Chapter")
            .AddColumn("Number").AsInt32();
    }
}
