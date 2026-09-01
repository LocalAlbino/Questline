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
}