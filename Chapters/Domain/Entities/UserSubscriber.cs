namespace Chapters.Domain.Entities;

public class UserSubscriber : BaseEntity<int>
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public int SubscriberId { get; set; }
    public User Subscriber { get; set; } = default!;
}