using System.ComponentModel.DataAnnotations;
using Data.Models.Abstractions;

namespace Data.Models;

public class AwayTeam : ModelBase
{
    [Required]
    public int TeamId { get; set; }

    public Team Team { get; set; }
}
