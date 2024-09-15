using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users;

    public UserInMemoryRepository()
    {
        users = new List<User>();
        AddUserAsync(new User{Username = "betelgeuse", Password = "first_password"});
        AddUserAsync(new User{Username = "orion", Password = "second_password"});
        AddUserAsync(new User{Username = "rigel", Password = "third_password"});
        AddUserAsync(new User{Username = "bohr", Password = "fourth_password"});
        AddUserAsync(new User{Username = "faraday", Password = "fifth_password"});
    }
    public async Task<User> AddUserAsync(User user)
    {
        user.UserId = users.Any() ? users.Max(u => u.UserId) + 1 : 1;
        users.Add(user);
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        User userToUpdate = GetUserByIdAsync(user.UserId).Result;
        
        users.Remove(userToUpdate);
        users.Add(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        User userToDelete = GetUserByIdAsync(userId).Result;
        users.Remove(userToDelete);

    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        User? foundUser = users.FirstOrDefault(u => u.UserId == userId);
        if (foundUser is null)
            throw new InvalidOperationException($"No user with ID {userId} found.");
        return foundUser;
    }

    public IQueryable<User> GetUsers()
    {
        return users.AsQueryable();
    }

    public async Task<bool> IsUsernameValidAsync(string? username)
    {
        return username is not null && !username.Trim().Equals("") && !users.Any(u => u.Username == username);
    }
}