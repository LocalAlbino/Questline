namespace Questline.Api.Models;

public class Board : BaseEntity
{
    public Guid UserId { get; set; }
    
    public required string Title { get; set; }

    public List<Group> Groups { get; } = new();
}