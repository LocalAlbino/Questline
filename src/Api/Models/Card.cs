namespace Questline.Api.Models;

public class Card : BaseEntity
{
    public long GroupId { get; set; }
    
    public Group? Group { get; set; }
    
    public long GameId { get; set; }
    
    public Game? Game { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public int Rank { get; set; }
}