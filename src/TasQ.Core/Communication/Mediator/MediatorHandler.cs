using MediatR;
using TasQ.Core.DomainObjects;
using TasQ.Core.Messages;
using TasQ.Core.Messages.CommonMessages.Notifications;

namespace TasQ.Core.Communication.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublicarEvento<T>(T evento) where T : Event
    {
        await _mediator.Publish(evento);
    }

    public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
    {
        await _mediator.Publish(notificacao);
    }

    public async Task<T> EnviarComando<T>(Command<T> comando)
        => await _mediator.Send(comando);
}