using Data.Models.Abstractions;
using FootyLeague.Data.Models.Abstractions;

namespace Data.Models;

public class Setting : ModelBase, IDeletableEntity
{
    public string Name { get; set; }

    public string Value { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateTime { get; set; }
}
