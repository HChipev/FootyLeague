using Data.Models;
using Data.Repository.Abstraction;

namespace Data.Repository;

public class TeamRepository : ITeamRepository
{
    private readonly MainContext _context;

    public TeamRepository(MainContext context)
    {
        _context = context;
    }

    public async Task DeleteTeamAsync(int id)
    {
        var team = await GetTeamByIdAsync(id);

        team.IsDeleted = true;
        team.DeletedDateTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task RestoreTeamAsync(int id)
    {
        var team = await GetTeamByIdAsync(id);

        team.IsDeleted = false;
        team.DeletedDateTime = null;
        await _context.SaveChangesAsync();
    }

    public IQueryable<Team> GetAllTeamsAsync()
    {
        return _context.Set<Team>().Where(x => !x.IsDeleted);
    }

    public IQueryable<Team> GetAllWithDeletedTeamsAsync()
    {
        return _context.Set<Team>();
    }

    public async Task<Team> GetTeamByIdAsync(int id)
    {
        return await _context.Set<Team>().FindAsync(id);
    }

    public async Task AddTeamAsync(Team team)
    {
        await _context.Set<Team>().AddAsync(team);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTeamAsync(Team team)
    {
        _context.Set<Team>().Update(team);
        await _context.SaveChangesAsync();
    }
}
