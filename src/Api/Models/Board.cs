namespace Questline.Api.Models;

public class Board : BaseEntity
{
    // Guid in string representation.
    public required string UserId { get; set; }

    public required string Title { get; set; }

    public List<Group> Groups { get; } = new();
}