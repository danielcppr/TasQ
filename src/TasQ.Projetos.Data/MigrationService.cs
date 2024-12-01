using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TasQ.Core.Data;

namespace TasQ.Projetos.Data;

public class MigrationService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecutarMigrationAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProjetoDbContext>();
        await context.Database.MigrateAsync();
    }

    public void Dispose() 
        => GC.SuppressFinalize(this);
}