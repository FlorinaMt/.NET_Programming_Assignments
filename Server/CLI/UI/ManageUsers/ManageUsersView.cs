using System.Security.Principal;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    public void Open(IUserRepository userRepository, CliApp cliApp)
    {
        string? userInput = Choose();

        while (userInput is null ||
               (!userInput.Equals("1") && !userInput.Equals("2")))
        {
            Console.WriteLine("Invalid input. Please try again.");
            userInput = Choose();
        }

        if (userInput.Equals("1"))
        {
            CreateUserView createUserView = new CreateUserView();
            createUserView.Open(userRepository, cliApp);
        }
        else if (userInput.Equals("2"))
        {
            UserListView userListView = new UserListView();
            userListView.Open(userRepository, cliApp);
        }
    }

    private string? Choose()
    {
        Console.Write("Choose 1 or 2: \n1. Create user. \n2. See users' list.");
        return Console.ReadLine();
    }
}