using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Questline.Api.Models;

namespace Questline.Api.Data;

public class AppDbContext(DbContextOptions options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Board> Boards => Set<Board>();

    public DbSet<Group> Groups => Set<Group>();

    public DbSet<Card> Cards => Set<Card>();

    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Serves both the ordered listing of a board's groups and the max-rank
        // lookup taken when a new group is appended.
        builder.Entity<Group>()
            .HasIndex(group => new { group.BoardId, group.Rank });
    }
}