#region usings

using Microsoft.EntityFrameworkCore;
using TasQ.Core.Communication.Mediator;
using TasQ.Core.Data;
using TasQ.Projetos.Domain;

#endregion

namespace TasQ.Projetos.Data;

public class ProjetoDbContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;

    public ProjetoDbContext(DbContextOptions<ProjetoDbContext> options, IMediatorHandler mediatorHandler) : base(options)
    {
        _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));

    }
    public DbSet<Projeto> Projetos { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<HistoricoTarefa> HistoricoTarefa { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        //modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjetoDbContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        //modelBuilder.HasSequence<int>("MinhaSequencia").StartsAt(1000).IncrementsBy(1);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> CommitAsync()
    {
        foreach (var entry in ChangeTracker.Entries()
                     .Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {
            if (entry.State == EntityState.Added) entry.Property("DataCadastro").CurrentValue = DateTime.UtcNow;

            if (entry.State == EntityState.Modified) entry.Property("DataCadastro").IsModified = false;
        }

        var sucesso = await base.SaveChangesAsync() > 0;
        //if (sucesso) await _mediatorHandler.PublicarEventos(this);
        if (sucesso) await _mediatorHandler.PublicarEventos(this);

        return sucesso;
    }
}