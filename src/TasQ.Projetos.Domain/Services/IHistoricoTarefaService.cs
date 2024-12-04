using TasQ.Projetos.Domain.Events;

namespace TasQ.Projetos.Domain.Services;

public interface IHistoricoTarefaService
{
    Task<bool> AdicionarHistoricoTarefa(TarefaAtualizadaEvent tarefa);
    Task<bool> AdicionarComentarioTarefa(Guid usuarioLogadoId, Guid tarefaId, string conteudo);
}