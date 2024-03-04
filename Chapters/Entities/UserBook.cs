using Chapters.Enums;

namespace Chapters.Entities;

public class UserBook : BaseEntity<int>
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public int BookId { get; set; }
    public Book Book { get; set; } = default!;
    
    public int UserRating { get; set; }
    public BookStatus BookStatus { get; set; }
}