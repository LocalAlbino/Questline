namespace Questline.Api.Models;

public class Game : BaseEntity
{
    public required string Title { get; set; }

    public string? BoxArt { get; set; }

    public int TimeToBeat { get; set; }
}