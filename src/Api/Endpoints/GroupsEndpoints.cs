using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Questline.Api.Data;
using Questline.Api.Dtos.Groups;
using Questline.Api.Extensions;

namespace Questline.Api.Endpoints;

public static class GroupsEndpoints
{
    public static void MapGroupsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/groups").RequireAuthorization();

        // GET api/groups/1
        group.MapGet("/{id:long}", async (long id, ClaimsPrincipal user, AppDbContext context) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            var groupDto = await context.Groups
                .Where(g => g.Id == id && g.Board!.UserId == userId)
                .Select(g => new GroupDto(g.Id, g.Title, g.Rank))
                .FirstOrDefaultAsync();

            return groupDto is null ? Results.NotFound() : Results.Ok(groupDto);
        }).WithName("GetGroup");
    }
}