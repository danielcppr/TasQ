using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TasQ.Core.Data;
using TasQ.Projetos.Domain;
using TasQ.Projetos.Domain.Interfaces;

namespace TasQ.Projetos.Data.Repositories;

public class ProjetoRepository : IProjetoRepository
{
    private readonly ProjetoDbContext _dbContext;
    public IUnitOfWork UnitOfWork => _dbContext;

    public ProjetoRepository(ProjetoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AdicionarAsync(Projeto projeto) => await _dbContext.Projetos.AddAsync(projeto);
    public Task Atualizar(Projeto projeto) => Task.FromResult(_dbContext.Projetos.Update(projeto));
    public async Task AdicionarAsync(Tarefa tarefa) => await _dbContext.Tarefas.AddAsync(tarefa);
    public Task Atualizar(Tarefa tarefa)
    { 
        _dbContext.Tarefas.Update(tarefa);
        return Task.CompletedTask;
    }
    public async Task AdicionarAsync(HistoricoTarefa historicoTarefa) => await _dbContext.HistoricoTarefa.AddAsync(historicoTarefa);
    public async Task<Projeto?> ObterPorId(Guid projetoId) => await _dbContext.Projetos.FindAsync(projetoId);
    public async Task<Tarefa?> ObterTarefaPorId(Guid tarefaId) => await _dbContext.Tarefas.FindAsync(tarefaId);

    public async Task<IEnumerable<Tarefa>> ObterTarefasConcluidasUsuarioLeitura(int filtroDias)
    {
        throw new NotImplementedException();
    } 

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}