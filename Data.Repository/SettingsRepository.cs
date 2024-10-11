using Data.Models;
using Data.Repository.Abstraction;

namespace Data.Repository;

public class SettingsRepository : ISettingsRepository
{
    private readonly MainContext _context;

    public SettingsRepository(MainContext context)
    {
        _context = context;
    }

    public IQueryable<Setting> GetAllSettingsAsync()
    {
        return _context.Set<Setting>().Where(x => !x.IsDeleted);
    }
}
