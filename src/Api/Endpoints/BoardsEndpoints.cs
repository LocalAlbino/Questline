using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Questline.Api.Data;
using Questline.Api.Dtos.Boards;
using Questline.Api.Extensions;
using Questline.Api.Models;

namespace Questline.Api.Endpoints;

public static class BoardsEndpoints
{
    public static void MapBoardsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/boards").RequireAuthorization();

        // GET api/boards/
        group.MapGet("/", async (ClaimsPrincipal user, AppDbContext context) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            return Results.Ok(await context.Boards
                .Where(board => board.UserId == userId)
                .OrderBy(board => board.CreatedAt)
                .Select(board => new BoardDto(board.Id, board.Title))
                .ToListAsync());
        });

        // GET api/boards/1
        group.MapGet("/{id:long}", async (long id, ClaimsPrincipal user, AppDbContext context) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            var board = await context.Boards
                .AsNoTracking()
                .FirstOrDefaultAsync(board => board.Id == id && board.UserId == userId);

            return board is null ? Results.NotFound() : Results.Ok(new BoardDto(board.Id, board.Title));
        }).WithName("GetBoard");

        // POST api/boards/
        group.MapPost("/", async (CreateBoardDto boardDto, ClaimsPrincipal user, AppDbContext context) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            var board = new Board
            {
                UserId = userId,
                Title = boardDto.Title
            };
            context.Boards.Add(board);

            await context.SaveChangesAsync();
            return Results.CreatedAtRoute("GetBoard", new { id = board.Id }, new BoardDto(board.Id, board.Title));
        });

        // PUT api/boards/1
        group.MapPut("/{id:long}", async (long id, UpdateBoardDto boardDto, ClaimsPrincipal user, AppDbContext context) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            var board = await context.Boards
                .FirstOrDefaultAsync(board => board.Id == id && board.UserId == userId);
            if (board is null) return Results.NotFound();

            board.Title = boardDto.Title;
            board.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE api/boards/1
        group.MapDelete("/{id:long}", async (long id, ClaimsPrincipal user, AppDbContext context) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            var deleted = await context.Boards
                .Where(board => board.UserId == userId && board.Id == id)
                .ExecuteDeleteAsync();

            return deleted == 0 ? Results.NotFound() : Results.NoContent();
        });
    }
}
