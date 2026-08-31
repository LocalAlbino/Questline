namespace Questline.Api.Models;

public class Experience : BaseEntity
{
    public Guid UserId { get; set; }
    
    public int Amount { get; set; }
}