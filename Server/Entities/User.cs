namespace Entities;

public class User
{
    public int UserId { get; set; }
    private string username, password;

    public List<Post>? Posts { get; set; } = [];
    public List<Comment>? Comments { get; set; } = [];
    public List<Like>? Likes { get; set; } = [];

    private User()
    {
    }

    public static User GetUser()
    {
        return new User();
    }

    public string Username
    {
        get { return username; }
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The username cannot be empty.");
            username = value;
        }
    }

    public string Password
    {
        get { return password; }
        set
        {
            if (value.Length < 5 || value.Trim().Equals(""))
                throw new ArgumentException("The password is too short.");
            password = value;
        }
    }


    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        User other = (User)obj;
        if (other.Username.Equals(Username) && other.Password.Equals(Password) &
            other.UserId == UserId)
            return true;
        return false;
    }
}