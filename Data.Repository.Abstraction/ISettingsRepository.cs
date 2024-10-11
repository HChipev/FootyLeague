using Data.Models;

namespace Data.Repository.Abstraction;

public interface ISettingsRepository
{
    IQueryable<Setting> GetAllSettingsAsync();
}
