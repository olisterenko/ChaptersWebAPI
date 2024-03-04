namespace Chapters.Entities;

public class Book : BaseEntity<int>
{
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public double Rating { get; set; }
    public int YearWritten { get; set; }
    public string? Cover { get; set; } = default;

    public List<UserBook> UserBooks { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<Chapter> Chapters { get; set; } = new();
}