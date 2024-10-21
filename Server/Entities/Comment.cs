namespace Entities;

public class Comment
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    private string? commentBody;

    public string CommentBody
    {
        get => commentBody;
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The comment cannot be empty.");
            commentBody = value;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        Comment other=(Comment)obj;
        if (other.CommentBody.Equals(commentBody) && other.UserId==UserId && other.PostId==PostId && other.CommentId==CommentId)
            return true;
        return false;
    }
}