using Chapters.Entities;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Chapters;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<UserBook> UserBooks => Set<UserBook>();
    public DbSet<UserRatingReview> UserRatingReviews => Set<UserRatingReview>();
    public DbSet<UserRatingComment> UserRatingComments => Set<UserRatingComment>();
    public DbSet<UserSubscriber> UserSubscribers => Set<UserSubscriber>();
    public DbSet<UserChapter> UserChapters => Set<UserChapter>();
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
}