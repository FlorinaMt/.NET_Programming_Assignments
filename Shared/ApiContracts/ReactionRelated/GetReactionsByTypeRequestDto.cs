namespace ApiContracts.ReactionRelated;

public class GetReactionsByTypeRequestDto
{
    public int CommentId { get; set; }
    public bool IsPositiveReaction { get; set; }
}