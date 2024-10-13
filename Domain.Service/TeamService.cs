using Api.ViewModels.Team;
using Data.Models;
using Data.Repository.Abstraction;
using Domain.Service.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class TeamService : ITeamService
{
    private readonly IMatchRepository _matchRepository;
    private readonly ITeamRepository _teamRepository;

    public TeamService(IMatchRepository matchRepository, ITeamRepository teamRepository)
    {
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
    }


    public async Task UpdateAllTeamsStats()
    {
        var matches = await _matchRepository.GetAllMatchesAsync().ToListAsync();
        var teams = await _teamRepository.GetAllTeamsAsync().ToListAsync();

        foreach (var match in matches)
        {
            var homeTeam = teams.FirstOrDefault(t => t.Id == match.HomeTeamId);
            var awayTeam = teams.FirstOrDefault(t => t.Id == match.AwayTeamId);

            if (homeTeam != null && awayTeam != null)
            {
                UpdateTeamStats(homeTeam, awayTeam, match);
            }

            await _teamRepository.UpdateTeamAsync(homeTeam);
            await _teamRepository.UpdateTeamAsync(awayTeam);
            await _matchRepository.UpdateMatchAsync(match);
        }
    }

    public async Task CreateTeamAsync(CreateTeamInputModel model)
    {
        var team = new Team
        {
            Name = model.Name
        };

        var teams = await _teamRepository.GetAllTeamsAsync().ToListAsync();

        if (teams.All(x => x.Name != model.Name))
        {
            await _teamRepository.AddTeamAsync(team);
        }
    }

    public async Task Delete(int id)
    {
        await _teamRepository.DeleteTeamAsync(id);
    }

    public async Task Restore(int id)
    {
        await _teamRepository.RestoreTeamAsync(id);

    }

    public async Task UpdateAsync(int id, EditTeamInputModel input)
    {
        var team = await _teamRepository.GetAllWithDeletedTeamsAsync().FirstOrDefaultAsync(x => x.Id == id);
        if (team == null)
        {
            return;
        }

        team.Name = input.Name;

        await _teamRepository.UpdateTeamAsync(team);
    }

    public async Task<IEnumerable<Team>> GetAllTeamsAsync()
    {
        var teams = await
            _teamRepository.GetAllTeamsAsync().ToListAsync();

        teams.Sort((x, y) => y.Points - x.Points);

        return teams;
    }

    public async Task<Team> GetTeamAsync(int id)
    {
        return await _teamRepository.GetTeamByIdAsync(id);
    }

    private void UpdateTeamStats(Team homeTeam, Team awayTeam, Match match)
    {
        homeTeam.Matches.Add(match);
        homeTeam.HomeMatches.Add(match);
        awayTeam.Matches.Add(match);
        awayTeam.AwayMatches.Add(match);

        if (!match.IsPlayed)
        {
            if (match.HomeTeamScore > match.AwayTeamScore)
            {
                homeTeam.Wins++;
                awayTeam.Losses++;
            }
            else if (match.HomeTeamScore == match.AwayTeamScore)
            {
                homeTeam.Draws++;
                awayTeam.Draws++;
            }
            else
            {
                homeTeam.Losses++;
                awayTeam.Wins++;
            }

            match.IsPlayed = true;
        }
    }
}
