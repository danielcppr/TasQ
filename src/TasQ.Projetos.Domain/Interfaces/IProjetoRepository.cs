using System.Linq.Expressions;
using TasQ.Core.Data;

namespace TasQ.Projetos.Domain.Interfaces;

public interface IProjetoRepository : IRepository<Projeto>
{

    Task AdicionarAsync(Projeto projeto);
    Task Atualizar(Projeto projeto);
    Task AdicionarAsync(Tarefa tarefa);
    Task Atualizar(Tarefa tarefa);
    Task AdicionarAsync(HistoricoTarefa historicoTarefa);
    Task<Projeto?> ObterPorId(Guid projetoId);
    Task<Tarefa?> ObterTarefaPorId(Guid tarefaId);


    #region Metodos de leitura, recomendado implementar com AsNoTracking()

    Task<IEnumerable<Tarefa>> ObterTarefasConcluidasUsuarioLeitura(int filtroDias);

    #endregion
}