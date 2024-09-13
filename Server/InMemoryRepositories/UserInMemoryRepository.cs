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
    public Task<User> AddUserAsync(User user)
    {
        user.UserId = users.Any() ? users.Max(u => u.UserId) + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateUserAsync(User user)
    {
        User userToUpdate = GetUserByIdAsync(user.UserId).Result;
        
        users.Remove(userToUpdate);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteUserAsync(int userId)
    {
        User userToDelete = GetUserByIdAsync(userId).Result;
        users.Remove(userToDelete);
        return Task.CompletedTask;

    }

    public Task<User> GetUserByIdAsync(int userId)
    {
        User? foundUser = users.FirstOrDefault(u => u.UserId == userId);
        if (foundUser is null)
            throw new InvalidOperationException($"No user with ID {userId} found.");
        return Task.FromResult(foundUser);
    }

    public IQueryable<User> GetUsers()
    {
        return users.AsQueryable();
    }
    public string ToString()
    {
        string s = "";
        for(int i=0; i<users.Count; i++)
            s=s+users[i].ToString()+'\n';
        return s;
    }

    public bool IsUsernameValid(string? username)
    {
        return !users.Any(u => u.Username == username);
    }
}