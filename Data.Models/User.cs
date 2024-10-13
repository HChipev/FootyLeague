using Data.Models.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Data.Models;

public class User : IdentityUser<int>, IBaseEntity, IDeletableEntity
{
    public User()
    {
        Roles = new HashSet<IdentityUserRole<int>>();
        Claims = new HashSet<IdentityUserClaim<int>>();
        Logins = new HashSet<IdentityUserLogin<int>>();
    }

    public ICollection<IdentityUserRole<int>> Roles { get; set; }

    public ICollection<IdentityUserClaim<int>> Claims { get; set; }

    public ICollection<IdentityUserLogin<int>> Logins { get; set; }

    public DateTime CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateTime { get; set; }
}
