using CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    public void Open(IUserRepository userRepository, CliApp cliApp)
    {
        string? username = EnterUsername();
        while (!userRepository.IsUsernameValid(username))
        {
            Console.WriteLine("Invalid username. Please try again.");
            EnterUsername();
        }

        string? password = EnterPassword();
        bool created = false;
        while (!created)
        {
            try
            {
                User newUser = userRepository.AddUserAsync(new User
                    { Username = username, Password = password }).Result;
                Console.WriteLine(
                    $"User created successfully: \n Username: {username}\n Password: {password}");
                created = true;
                
                cliApp.LoadManagePostsView(newUser);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                username = EnterUsername();
                password = EnterPassword();
            }
        }
    }
    
    private string? EnterUsername()
    {
        Console.WriteLine("Enter username: ");
        return Console.ReadLine();
    }
    
    private string? EnterPassword()
    {
        Console.WriteLine("Enter password: ");
        return Console.ReadLine();
    }
    
}