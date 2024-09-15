namespace Entities;

public class Post
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    private string? title, body;

    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The title cannot be empty.");
            title = value;
        }
    }

    public string Body {
        get => body;
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The body cannot be empty.");
            body = value;
        }
    }
  
}