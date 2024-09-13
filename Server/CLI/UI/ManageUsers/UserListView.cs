using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UserListView
{
    public void Open(IUserRepository userRepository, CliApp cliApp)
    {
        Console.WriteLine("The list of users:");
        for(int i=0; i<userRepository.GetUsers().Count(); i++)
         Console.WriteLine(userRepository.GetUsers().ElementAt(i).Username);

        string? userInput;
        do
        {
            userInput = GoBackToManageUsersView();
        } while (userInput is null || !userInput.Equals("x"));
        cliApp.LoadManageUsersView();
    }

    private string? GoBackToManageUsersView()
    {
        Console.WriteLine("To go back, enter x.");
        return Console.ReadLine();
    }
}