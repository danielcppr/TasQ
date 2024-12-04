using TasQ.Core.DomainObjects;
using TasQ.Projetos.Domain.Events;
using TasQ.Projetos.Domain.Interfaces;

namespace TasQ.Projetos.Domain.Services;

public class HistoricoTarefaService : IHistoricoTarefaService
{
    private readonly IProjetoRepository _projetoRepository;

    public HistoricoTarefaService(IProjetoRepository projetoRepository)
    {
        _projetoRepository = projetoRepository;
    }

    public async Task<bool> AdicionarHistoricoTarefa(TarefaAtualizadaEvent tarefa)
    {
        await _projetoRepository.AdicionarAsync(new HistoricoTarefa(tarefa.UsuarioLogadoId, tarefa.TarefaId,
            new HistoricoItem(tarefa.Tipo, tarefa.ValorNovo)));

        return await _projetoRepository.UnitOfWork.CommitAsync();
    }

    public async Task<bool> AdicionarComentarioTarefa(Guid usuarioLogadoId, Guid tarefaId, string conteudo)
    {
        var tarefa = await _projetoRepository.ObterTarefaPorId(tarefaId);

        if (string.IsNullOrEmpty(conteudo))
            return await Task.FromResult(false); // Todo: add notificacao

        if (usuarioLogadoId.Equals(Guid.Empty))
            return await Task.FromResult(false);

        if (tarefa.EhNullOuRemovido())
            return await Task.FromResult(false);

        var historicoTarefa = new HistoricoTarefa(usuarioLogadoId, tarefa.Id,
            new HistoricoItem(TipoAtualizacaoTarefaEnum.COMENTARIO, conteudo));

        await _projetoRepository.AdicionarAsync(historicoTarefa);
        return await _projetoRepository.UnitOfWork.CommitAsync();
    }
}