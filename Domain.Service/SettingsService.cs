using Data.Models;
using Data.Repository.Abstraction;
using Domain.Service.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class SettingsService : ISettingsService
{
    private readonly ISettingsRepository _settingsRepository;

    public SettingsService(ISettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public int GetCount()
    {
        return _settingsRepository.GetAllSettingsAsync().Count();
    }

    public async Task<IEnumerable<Setting>> GetAllAsync()
    {
        return await _settingsRepository.GetAllSettingsAsync().ToListAsync();
    }
}
