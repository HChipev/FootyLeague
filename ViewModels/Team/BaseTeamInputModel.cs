using System.ComponentModel.DataAnnotations;

namespace ViewModels.Team;

public class BaseTeamInputModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}
