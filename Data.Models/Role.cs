using Data.Models.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Data.Models;

public class Role : IdentityRole<int>, IBaseEntity, IDeletableEntity
{
    public DateTime CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateTime { get; set; }
}
