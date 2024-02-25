using Chapters.Entities;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Chapters;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    
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