using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    User AddUser(User user);
    User UpdateUser(User user);
    User DeleteUser(User user);
    User GetUserById(int userId);
    List<User> GetUsers();
}