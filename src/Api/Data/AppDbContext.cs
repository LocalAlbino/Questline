using Microsoft.EntityFrameworkCore;
using Questline.Api.Models;

namespace Questline.Api.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Board> Boards => Set<Board>();

    public DbSet<Group> Groups => Set<Group>();

    public DbSet<Card> Cards => Set<Card>();

    public DbSet<Game> Games => Set<Game>();
    
    public DbSet<Experience> Experiences => Set<Experience>();
}