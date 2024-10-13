using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels.Team;

public class BaseTeamInputModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}
