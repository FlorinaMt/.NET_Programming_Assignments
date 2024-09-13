using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UserListView
{
    public void Open(IUserRepository userRepository, CliApp cliApp)
    {
        Console.WriteLine("The list of users: \n" + userRepository.GetUsers());

        string? userInput = GoBackToManageUsersView();
        while (userInput is null || !userInput.Equals("x"))
            GoBackToManageUsersView();
        cliApp.LoadManageUsersView();
    }

    private string? GoBackToManageUsersView()
    {
        Console.WriteLine("To go back, enter x.");
        return Console.ReadLine();
    }
}