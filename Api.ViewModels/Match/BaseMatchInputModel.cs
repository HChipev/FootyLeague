using Api.ViewModels.Team;

namespace Api.ViewModels.Match;

public class BaseMatchInputModel
{
    public int HomeTeamScore { get; set; }

    public int AwayTeamScore { get; set; }

    public CreateMatchTeamViewModel HomeTeam { get; set; }

    public CreateMatchTeamViewModel AwayTeam { get; set; }

    public bool IsPlayed { get; set; }
}
