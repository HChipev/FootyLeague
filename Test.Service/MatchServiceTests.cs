using Data.Repository.Abstraction;
using Moq;
using Services;
using Xunit;
using Match = Data.Models.Match;

namespace Test.Service;

public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock;
    private readonly MatchService _matchService;
    private readonly Mock<ITeamRepository> _teamRepositoryMock;

    public MatchServiceTests()
    {
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _matchService = new MatchService(_matchRepositoryMock.Object, _teamRepositoryMock.Object);
    }

    [Fact]
    public async Task GetMatchAsync_ValidId_ReturnsMatch()
    {
        // Arrange
        var matchId = 1;
        var expectedMatch = new Match
        {
            Id = matchId,
            HomeTeamScore = 2,
            AwayTeamScore = 1,
            DateTime = DateTime.Now,
            HomeTeamId = 1,
            AwayTeamId = 2,
            IsPlayed = true,
            IsDeleted = false
        };

        _matchRepositoryMock.Setup(repo => repo.GetMatchByIdAsync(matchId))
            .ReturnsAsync(expectedMatch);

        // Act
        var result = await _matchService.GetMatchAsync(matchId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedMatch.HomeTeamScore, result.HomeTeamScore);
        Assert.Equal(expectedMatch.AwayTeamScore, result.AwayTeamScore);
        Assert.True(result.IsPlayed);
        Assert.False(result.IsDeleted);
    }

    [Fact]
    public async Task GetMatchAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        var matchId = 99; // Assuming this ID does not exist
        _matchRepositoryMock.Setup(repo => repo.GetMatchByIdAsync(matchId))
            .ReturnsAsync((Match)null);

        // Act
        var result = await _matchService.GetMatchAsync(matchId);

        // Assert
        Assert.Null(result);
    }
}
