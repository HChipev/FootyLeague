using System.ComponentModel.DataAnnotations;
using Data.Models.Abstractions;

namespace Data.Models;

public class Team : ModelBase, IDeletableEntity
{
    public Team()
    {
        Matches = new HashSet<Match>();
        HomeMatches = new HashSet<Match>();
        AwayMatches = new HashSet<Match>();

    }

    [Required]
    [MinLength(3)]
    public string Name { get; set; }

    public int Points => Wins * 3 + Draws;

    public ICollection<Match> Matches { get; set; }

    public ICollection<Match> HomeMatches { get; set; }

    public ICollection<Match> AwayMatches { get; set; }

    public int Wins { get; set; }

    public int Draws { get; set; }

    public int Losses { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateTime { get; set; }
}
