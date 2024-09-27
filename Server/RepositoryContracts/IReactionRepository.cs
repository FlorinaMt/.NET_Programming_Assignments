using Entities;

namespace RepositoryContracts;

public interface IReactionRepository
{
    Task<Reaction> AddReactionAsync(Reaction reaction);
    Task DeleteReactionAsync(Reaction reaction);
    Task<Reaction> GetReactionByIdAsync(int id);
    IQueryable<Reaction> GetReactionsForComment(int commentId);
    IQueryable<Reaction> GetAllReactions();
}