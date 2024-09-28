namespace ApiContracts.ReactionRelated;

public class GetReactionsByTypeRequestDto
{
    public required int CommentId { get; set; }
    public bool IsPositiveReaction { get; set; }
}