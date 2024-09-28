namespace ApiContracts.ReactionRelated;

//used for response in both POST and GET 
public class GetReactionResponseDto
{
    public int ReactionId { get; set; }
    public int CommentId { get; set; }
    public string AuthorUsername { get; set; }
    public bool IsPositive { get; set; }
}