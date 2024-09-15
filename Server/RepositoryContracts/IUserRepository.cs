using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int userId);
    Task <User> GetUserByIdAsync(int userId);
    IQueryable<User> GetUsers();
    Task<bool> IsUsernameValidAsync(string username);
}