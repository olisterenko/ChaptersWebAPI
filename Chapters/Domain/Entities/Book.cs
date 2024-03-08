namespace Chapters.Domain.Entities;

public class Book : BaseEntity<int>
{
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    
    // TODO: выпилить рейтинг
    public double Rating { get; set; }
    public int YearWritten { get; set; }
    public string? Cover { get; set; } = default;

    public List<UserBook> UserBooks { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<Chapter> Chapters { get; set; } = new();
}