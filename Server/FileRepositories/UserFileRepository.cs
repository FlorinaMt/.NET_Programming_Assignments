using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");

            int delay = 100;
            Thread.Sleep(delay);
            AddUserAsync(new User
                { Username = "betelgeuse", Password = "first_password" });
            Thread.Sleep(delay);
            AddUserAsync(new User
                { Username = "orion", Password = "second_password" });
            Thread.Sleep(delay);
            AddUserAsync(new User
                { Username = "rigel", Password = "third_password" });
            Thread.Sleep(delay);
            AddUserAsync(new User
                { Username = "bohr", Password = "fourth_password" });
            Thread.Sleep(delay);
            AddUserAsync(new User
                { Username = "faraday", Password = "fifth_password" });
        }
    }

    public async Task<User> AddUserAsync(User user)
    {
        List<User> users = await LoadUsersAsync();
        int maxId = users.Count > 0 ? users.Max(like => like.UserId) : 0;
        user.UserId = maxId + 1;
        users.Add(user);
        SaveUsersAsync(users);
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        List<User> users = await LoadUsersAsync();
        User userToUpdate = await GetUserByIdAsync(user.UserId);
        users.Remove(userToUpdate);
        users.Add(user);
        SaveUsersAsync(users);
    }

    public async Task DeleteUserAsync(int userId)
    {
        List<User> users = await LoadUsersAsync();
        User userToDelete = await GetUserByIdAsync(userId);
        users.Remove(userToDelete);
        SaveUsersAsync(users);
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        List<User> users = await LoadUsersAsync();
        User? user = users.SingleOrDefault(l => l.UserId == userId);
        if (user is null)
            throw new InvalidOperationException(
                $"User with ID {user} not found.");
        return user;
    }

    public IQueryable<User> GetUsers()
    {
        List<User> users = LoadUsersAsync().Result;
        return users.AsQueryable();
    }

    public async Task<bool> IsUsernameValidAsync(string? username)
    {
        List<User> users = await LoadUsersAsync();
        return username is not null && !username.Trim().Equals("") &&
               users.All(u => u.Username != username);
    }

    private async Task<List<User>> LoadUsersAsync()
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users =
            JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users;
    }

    private async void SaveUsersAsync(List<User> toSaveUsers)
    {
        string usersAsJson = JsonSerializer.Serialize(toSaveUsers,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }
}