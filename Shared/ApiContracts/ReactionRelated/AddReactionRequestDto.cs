namespace ApiContracts.ReactionRelated;

public class AddReactionRequestDto
{
    public int UserId { get; set; }
    public int CommentId { get; set; }
    public bool IsPositive { get; set; }
}