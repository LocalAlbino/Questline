using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Questline.Api.Data;
using Questline.Api.Dtos.Groups;
using Questline.Api.Extensions;
using Questline.Api.Models;

namespace Questline.Api.Endpoints;

public static class BoardsGroupsEndpoints
{
    // Ranks are spaced out so a group can be dropped between two others by
    // averaging their ranks, instead of renumbering the whole board.
    private const int RankGap = 1024;

    public static void MapBoardsGroupsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/boards/{id:long}/groups").RequireAuthorization();

        // GET api/boards/1/groups
        group.MapGet("/", async (long id, ClaimsPrincipal user, AppDbContext context) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();
            
            var board = await context.Boards
                .FirstOrDefaultAsync(board => board.Id == id && board.UserId == userId);
            if (board is null) return Results.NotFound();

            return Results.Ok(await context.Groups
                .Where(g => g.BoardId == board.Id)
                .OrderBy(g => g.Rank)
                .ThenBy(g => g.Id)
                .Select(g => new GroupDto(g.Id, g.Title, g.Rank))
                .ToListAsync());
        });
        
        // POST api/boards/1/groups
        group.MapPost("/", async (long id, CreateGroupDto request, ClaimsPrincipal user, AppDbContext context) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();
            
            var board = await context.Boards
                .Where(board => board.Id == id && board.UserId == userId)
                .Select(board => new { board.Id, MaxRank = board.Groups.Max(g => (int?)g.Rank) })
                .FirstOrDefaultAsync();
            if (board is null) return Results.NotFound();

            var g = new Group
            {
                BoardId = board.Id,
                Title = request.Title,
                Rank = (board.MaxRank ?? 0) + RankGap,
            };
            
            context.Groups.Add(g);
            await context.SaveChangesAsync();
            return Results.CreatedAtRoute("GetGroup",  new { id = g.Id }, new GroupDto(g.Id, g.Title, g.Rank));
        });
    }
}