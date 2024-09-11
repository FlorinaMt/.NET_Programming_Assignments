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
            if (value is null)
                throw new ArgumentException("The comment cannot be empty.");
            commentBody = value;
        }
    }

    public string ToString()
    {
        return($"CommentId = {CommentId}, UserId = {UserId}, PostId = {PostId}, CommentBody = {CommentBody}");
    }
}