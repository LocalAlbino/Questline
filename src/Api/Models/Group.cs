namespace Questline.Api.Models;

public class Group : BaseEntity
{
    public long BoardId { get; set; }
    
    public Board? Board { get; set; }
    
    public required string Title { get; set; }
    
    public int Rank { get; set; }

    public List<Card> Cards { get; } = new();
}