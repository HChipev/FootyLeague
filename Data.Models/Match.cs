using System.ComponentModel.DataAnnotations;
using Data.Models.Abstractions;
using FootyLeague.Data.Models.Abstractions;

namespace Data.Models;

public class Match : ModelBase, IDeletableEntity
{
    [Required]
    public int HomeTeamScore { get; set; }

    [Required]
    public int AwayTeamScore { get; set; }

    [Required]
    public DateTime DateTime { get; set; }

    [Required]
    public int HomeTeamId { get; set; }

    [Required]
    public Team HomeTeam { get; set; }

    [Required]
    public int AwayTeamId { get; set; }

    [Required]
    public Team AwayTeam { get; set; }

    [Required]
    public bool IsPlayed { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateTime { get; set; }
}
