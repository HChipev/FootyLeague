using Data.Models;

namespace Data.Repository.Abstraction;

public interface ITeamRepository
{
    IQueryable<Team> GetAllTeamsAsync();
    IQueryable<Team> GetAllWithDeletedTeamsAsync();
    Task<Team> GetTeamByIdAsync(int id);
    Task AddTeamAsync(Team team);
    Task UpdateTeamAsync(Team team);
    Task DeleteTeamAsync(int id);
    Task RestoreTeamAsync(int id);
}
