namespace Entities;

public class Reaction
{
    public int ReactionId { get; set; }
    public int UserId { get; set; }
    public int CommentId { get; set; }
    public bool IsPositive { get; set; }
}