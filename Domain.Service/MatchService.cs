using Data.Models;
using Data.Repository.Abstraction;
using Domain.Service.Abstraction;
using Microsoft.EntityFrameworkCore;
using ViewModels.Match;

namespace Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly ITeamRepository _teamRepository;

    public MatchService(IMatchRepository matchRepository, ITeamRepository teamRepository)
    {
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
    }


    public async Task<IEnumerable<Match>> GetAllMatchesAsync()
    {
        var query = _matchRepository.GetAllMatchesAsync().OrderBy(x => x.DateTime);

        return await query.ToListAsync();
    }

    public async Task CreateMatchAsync(CreateMatchInputModel model)
    {
        var homeTeam = await _teamRepository.GetAllTeamsAsync().FirstOrDefaultAsync(x => x.Id == model.HomeTeam.Id);
        var awayTeam = await _teamRepository.GetAllTeamsAsync().FirstOrDefaultAsync(x => x.Id == model.AwayTeam.Id);

        if (homeTeam == null || awayTeam == null)
        {
            return;
        }

        var match = new Match
        {
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            IsPlayed = model.IsPlayed,
            AwayTeamScore = model.AwayTeamScore,
            HomeTeamScore = model.HomeTeamScore,
            DateTime = DateTime.UtcNow

        };

        var findMatch = await _matchRepository.GetAllMatchesAsync().FirstOrDefaultAsync(x => x == match);

        if (findMatch == null)
        {
            await _matchRepository.AddMatchAsync(match);
        }
    }

    public async Task Delete(int id)
    {
        await _matchRepository.DeleteMatchAsync(id);
    }

    public async Task Restore(int id)
    {
        await _matchRepository.RestoreMatchAsync(id);
    }

    public async Task UpdateAsync(int id, EditMatchInputModel input)
    {
        var match = await _matchRepository.GetAllWithDeletedMatchesAsync().FirstOrDefaultAsync(x => x.Id == id);
        if (match == null)
        {
            return;
        }

        var homeTeam = await _teamRepository.GetAllWithDeletedTeamsAsync().FirstOrDefaultAsync(x => x.Id == input.HomeTeam.Id);
        if (homeTeam == null)
        {
            return;
        }

        var awayTeam = await _teamRepository.GetAllWithDeletedTeamsAsync().FirstOrDefaultAsync(x => x.Id == input.AwayTeam.Id);
        if (awayTeam == null)
        {
            return;
        }

        match.HomeTeamScore = input.HomeTeamScore;
        match.AwayTeamScore = input.AwayTeamScore;
        match.HomeTeam = homeTeam;
        match.AwayTeam = awayTeam;
        match.IsPlayed = input.IsPlayed;

        await _matchRepository.UpdateMatchAsync(match);
    }

    public async Task<Match> GetMatchAsync(int id)
    {
        return await _matchRepository.GetMatchByIdAsync(id);
    }
}
