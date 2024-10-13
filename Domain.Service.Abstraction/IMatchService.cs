using Api.ViewModels.Match;
using Data.Models;

namespace Domain.Service.Abstraction;

public interface IMatchService
{
    Task<IEnumerable<Match>> GetAllMatchesAsync();

    Task CreateMatchAsync(CreateMatchInputModel model);

    Task<Match> GetMatchAsync(int id);

    Task Delete(int id);

    Task Restore(int id);

    Task UpdateAsync(int id, EditMatchInputModel input);
}
