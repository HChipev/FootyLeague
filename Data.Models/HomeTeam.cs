using System.ComponentModel.DataAnnotations;
using Data.Models.Abstractions;
using FootyLeague.Data.Models;

namespace Data.Models;

public class HomeTeam : ModelBase
{
    [Required]
    public int TeamId { get; set; }

    public Team Team { get; set; }
}
