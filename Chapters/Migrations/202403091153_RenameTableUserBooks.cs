using FluentMigrator;

namespace Chapters;

[TimestampedMigration(2024, 3, 9, 11, 53)]
public class RenameTableUserBooks : ForwardOnlyMigration {
    public override void Up()
    {
        Rename.Table("UserBooks").To("UserBook");
    }
}
