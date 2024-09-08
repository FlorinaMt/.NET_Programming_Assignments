namespace Entities;

public class Comment
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    private string commentBody;

    public string CommentBody
    {
        get
        {
            return commentBody;
        }
        set
        {
            if (value == null)
                throw new ArgumentException("The comment cannot be empty.");
        }
    }
}