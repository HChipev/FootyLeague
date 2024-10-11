namespace Data.Seeds;

public interface IDatabaseSeeder
{
    Task Seed(IServiceProvider serviceProvider);
}
