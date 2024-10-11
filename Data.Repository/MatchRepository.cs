using Data.Models;
using Data.Repository.Abstraction;

namespace Data.Repository;

public class MatchRepository : IMatchRepository
{
    private readonly MainContext _context;

    public MatchRepository(MainContext context)
    {
        _context = context;
    }

    public async Task DeleteMatchAsync(int id)
    {
        var match = await GetMatchByIdAsync(id);

        match.IsDeleted = true;
        match.DeletedDateTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task RestoreMatchAsync(int id)
    {
        var match = await GetMatchByIdAsync(id);

        match.IsDeleted = false;
        match.DeletedDateTime = null;
        await _context.SaveChangesAsync();
    }

    public IQueryable<Match> GetAllMatchesAsync()
    {
        return _context.Set<Match>().Where(x => !x.IsDeleted);
    }

    public IQueryable<Match> GetAllWithDeletedMatchesAsync()
    {
        return _context.Set<Match>();
    }

    public async Task<Match> GetMatchByIdAsync(int id)
    {
        return await _context.Set<Match>().FindAsync(id);
    }

    public async Task AddMatchAsync(Match match)
    {
        await _context.Set<Match>().AddAsync(match);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMatchAsync(Match match)
    {
        _context.Set<Match>().Update(match);
        await _context.SaveChangesAsync();
    }
}
