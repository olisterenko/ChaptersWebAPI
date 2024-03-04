namespace Chapters.Entities;

public class Chapter : BaseEntity<int>
{
    public int BookId { get; set; }
    public Book Book { get; set; } = default!;
    public string Title { get; set; } = default!;
    
    public List<UserChapter> UserChapters { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
}