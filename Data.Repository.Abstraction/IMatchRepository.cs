using Data.Models;

namespace Data.Repository.Abstraction;

public interface IMatchRepository
{
    IQueryable<Match> GetAllMatchesAsync();
    IQueryable<Match> GetAllWithDeletedMatchesAsync();
    Task<Match> GetMatchByIdAsync(int id);
    Task AddMatchAsync(Match match);
    Task UpdateMatchAsync(Match match);
    Task DeleteMatchAsync(int id);
    Task RestoreMatchAsync(int id);
}
