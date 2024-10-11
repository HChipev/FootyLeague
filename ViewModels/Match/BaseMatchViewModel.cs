using ViewModels.Team;

namespace ViewModels.Match;

public class BaseMatchViewModel
{
    public int MatchId { get; set; }

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public int HomeTeamScore { get; set; }

    public int AwayTeamScore { get; set; }

    public TeamViewModel HomeTeam { get; set; }

    public TeamViewModel AwayTeam { get; set; }

    public bool IsPlayed { get; set; }
}
