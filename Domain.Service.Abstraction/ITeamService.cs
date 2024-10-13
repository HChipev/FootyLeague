using Api.ViewModels.Team;
using Data.Models;

namespace Domain.Service.Abstraction;

public interface ITeamService
{
    Task<IEnumerable<Team>> GetAllTeamsAsync();

    Task UpdateAllTeamsStats();

    Task CreateTeamAsync(CreateTeamInputModel model);

    Task<Team> GetTeamAsync(int id);

    Task Delete(int id);

    Task Restore(int id);

    Task UpdateAsync(int id, EditTeamInputModel input);
}
