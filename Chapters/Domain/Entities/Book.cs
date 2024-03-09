namespace Chapters.Domain.Entities;

public class Book : BaseEntity<int>
{
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;

    public double Rating { get; set; }
    public int YearWritten { get; set; }
    public string? Cover { get; set; }

    public List<UserBook> UserBooks { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
    public List<Chapter> Chapters { get; set; } = [];
}