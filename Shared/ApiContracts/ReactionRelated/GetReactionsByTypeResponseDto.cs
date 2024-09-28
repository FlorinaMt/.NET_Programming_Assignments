namespace ApiContracts.ReactionRelated;

public class GetReactionsByTypeResponseDto
{
    public required int CommentId { get; set; }
    public required int PositiveReactionsCount { get; set; }
    public required int NegativeReactionsCount { get; set; }
}