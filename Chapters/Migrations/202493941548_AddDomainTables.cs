using Chapters.Enums;
using FluentMigrator;

namespace Chapters;

[TimestampedMigration(2024, 3, 4, 15, 48)]
public class AddDomainTables : ForwardOnlyMigration {
    public override void Up()
    {
        Create.Table("Book")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Title").AsString(255).NotNullable().Unique()
            .WithColumn("Author").AsString(255).NotNullable()
            .WithColumn("Rating").AsDouble().WithDefaultValue(0)
            .WithColumn("YearWritten").AsInt16().NotNullable()
            .WithColumn("Cover").AsString(255);

        Create.Table("UserBooks")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("BookId").AsInt32().NotNullable().ForeignKey("Book", "Id")
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("User", "Id")
            .WithColumn("UserRating").AsInt32().NotNullable()
            .WithColumn("BookStatus").AsInt32().NotNullable().WithDefaultValue((int)BookStatus.NotStarted);

        Create.Table("Review")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("BookId").AsInt32().NotNullable().ForeignKey("Book", "Id")
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("Text").AsString(10000).NotNullable()
            .WithColumn("AuthorId").AsInt32().NotNullable().ForeignKey("User", "Id")
            .WithColumn("Rating").AsInt32().WithDefaultValue(0)
            .WithColumn("CreatedAt").AsDateTimeOffset().WithDefault(SystemMethods.CurrentDateTime);

        Create.Table("UserRatingReview")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("ReviewId").AsInt32().NotNullable().ForeignKey("Review", "Id")
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("User", "Id")
            .WithColumn("UserRating").AsInt32().NotNullable();

        Create.Table("Chapter")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("BookId").AsInt32().NotNullable().ForeignKey("Book", "Id")
            .WithColumn("Title").AsString(255).NotNullable();

        Create.Table("UserChapter")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("ChapterId").AsInt32().NotNullable().ForeignKey("Chapter", "Id")
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("User", "Id")
            .WithColumn("UserRating").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("IsRead").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("ReadTime").AsDateTimeOffset();

        Create.Table("Comment")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("ChapterId").AsInt32().NotNullable().ForeignKey("Chapter", "Id")
            .WithColumn("AuthorId").AsInt32().NotNullable().ForeignKey("User", "Id")
            .WithColumn("Text").AsString(10000).NotNullable()
            .WithColumn("Rating").AsInt32().WithDefaultValue(0)
            .WithColumn("CreatedAt").AsDateTimeOffset().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("UserRatingComment")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("CommentId").AsInt32().NotNullable().ForeignKey("Comment", "Id")
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("User", "Id")
            .WithColumn("UserRating").AsInt32().NotNullable();

        Create.Table("UserSubscriber")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("SubscriberId").AsInt32().NotNullable().ForeignKey("User", "Id")
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("User", "Id");
    }
}
