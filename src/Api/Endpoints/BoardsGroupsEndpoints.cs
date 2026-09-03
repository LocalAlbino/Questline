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
            
            // Projecting the groups out of the board keeps ownership and the listing in
            // one round trip. A null result means no such board; an empty list means a
            // board that has no groups yet.
            var groups = await context.Boards
                .Where(board => board.Id == id && board.UserId == userId)
                .Select(board => board.Groups
                    .OrderBy(g => g.Rank)
                    .ThenBy(g => g.Id)
                    .Select(g => new GroupDto(g.Id, g.Title, g.Rank))
                    .ToList())
                .FirstOrDefaultAsync();

            return groups is null ? Results.NotFound() : Results.Ok(groups);
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