namespace ViewModels.Administration.Users;

public class BaseUserViewModel
{
    protected BaseUserViewModel()
    {
        Roles = new HashSet<UserRoleViewModel>();
    }

    public int Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public ICollection<UserRoleViewModel> Roles { get; set; }
}
