namespace ApiContracts.ReactionRelated;

public class GetReactionsByTypeResponseDto
{
    public int CommentId { get; set; }
    public int PositiveReactionsCount { get; set; }
    public int NegativeReactionsCount { get; set; }
}