namespace ApiContracts.ReactionRelated;

public class AddReactionRequestDto
{
    public required int UserId { get; set; }
    public required int CommentId { get; set; }
    public required bool IsPositive { get; set; }
}