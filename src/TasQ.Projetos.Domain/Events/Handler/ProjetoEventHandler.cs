using MediatR;
using TasQ.Projetos.Domain.Services;

namespace TasQ.Projetos.Domain.Events.Handler;

public class ProjetoEventHandler :
    INotificationHandler<TarefaAtualizadaEvent>
{
    private readonly IHistoricoTarefaService _historicoTarefaService;

    public ProjetoEventHandler(IHistoricoTarefaService historicoTarefaService)
    {
        _historicoTarefaService = historicoTarefaService;
    }
    public async Task Handle(TarefaAtualizadaEvent evento, CancellationToken cancellationToken)
    {
        // Envio email, notificacao, outros fluxos...

        await _historicoTarefaService.AdicionarHistoricoTarefa(evento);
    }
}