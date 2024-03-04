﻿namespace Chapters.Entities;

public class Comment : BaseEntity<int>
{
    public int AuthorId { get; set; }
    public User Author { get; set; } = default!;
    
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
    public int Rating { get; set; }
    
    public List<UserRatingComment> UserRatingComments { get; set; } = default!;
}
