using Microsoft.AspNetCore.Identity;

namespace Questline.Api.Models;

public class AppUser : IdentityUser<Guid>
{
    public int Experience { get; set; }
}