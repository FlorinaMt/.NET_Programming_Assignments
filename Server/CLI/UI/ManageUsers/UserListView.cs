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
    public void Open()
    {
        Console.WriteLine("The list of users:");
        for(int i=0; i<userRepository.GetUsers().Count(); i++)
         Console.WriteLine(userRepository.GetUsers().ElementAt(i).Username);

        string? userInput;
        do
        {
            userInput = GoBackToManageUsersView();
        } while (userInput is null || !userInput.Equals("x"));
        manageUsersView.Open();
    }

    private string? GoBackToManageUsersView()
    {
        Console.WriteLine("To go back, enter x.");
        return Console.ReadLine();
    }
}