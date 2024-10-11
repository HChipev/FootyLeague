using Data.Models;

namespace Domain.Service.Abstraction;

public interface ISettingsService
{
    int GetCount();

    Task<IEnumerable<Setting>> GetAllAsync();
}
