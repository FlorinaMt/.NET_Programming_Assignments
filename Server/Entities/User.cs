namespace Entities;

public class User
{
    public int UserId { get; set; }
    private string username, password;
    public string Username {
        get
        {
            return username;
        }
        set
        {
            if(value is null)
                throw new ArgumentException("The username cannot be empty.");
            username = value;
        }
    }

    public string Password { 
        get
        {
            return password;
        }
        set
        {
            if(value.Length<5)
                throw new ArgumentException("The password is too short.");
            username = value;
        }
        
    }
}