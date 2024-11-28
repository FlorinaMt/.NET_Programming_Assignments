using System.Globalization;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext context;

    public EfcUserRepository(AppContext context)
    {
        this.context = context;
    }

    public async Task<User> AddUserAsync(User user)
    {
        EntityEntry<User> entityEntry = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateUserAsync(User user)
    {
        if (!await context.Users.AnyAsync((u => u.UserId == user.UserId)))
            throw new ArgumentException(
                $"User with ID {user.UserId} does not exist");

        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        User? user =
            await context.Users.SingleOrDefaultAsync(u => u.UserId == userId);
        if (user is null)
            throw new ArgumentException(
                $"User with ID {userId} does not exist");

        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        User? user =
            await context.Users.SingleOrDefaultAsync(u => u.UserId == userId);
        if (user is null)
            throw new ArgumentException(
                $"User with ID {userId} not found");
        return user;
    }

    public IQueryable<User> GetUsers()
    {
        return  context.Users.AsQueryable();
    }
    
}