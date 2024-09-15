using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UserListView
{
    private IUserRepository userRepository;
    private ManageUsersView manageUsersView;

    public UserListView(IUserRepository userRepository,
        ManageUsersView manageUsersView)
    {
        this.userRepository = userRepository;
        this.manageUsersView = manageUsersView;
    }
    public async Task OpenAsync()
    {
        Console.WriteLine("The list of users:");
        for(var i=0; i<userRepository.GetUsers().Count(); i++)
         Console.WriteLine(userRepository.GetUsers().ElementAt(i).Username);

        string? userInput;
        do
        {
            userInput = GoBackToManageUsersView();
        } while (userInput is null || !userInput.Equals("x"));
        await manageUsersView.OpenAsync();
    }

    private string? GoBackToManageUsersView()
    {
        Console.WriteLine("To go back, enter x.");
        return Console.ReadLine();
    }
}