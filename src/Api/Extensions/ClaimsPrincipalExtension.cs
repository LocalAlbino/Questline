using System.Security.Claims;

namespace Questline.Api.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string? GetUserId(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);
}
