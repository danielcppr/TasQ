using TasQ.Core.Messages;
using TasQ.Core.Messages.CommonMessages.Notifications;

namespace TasQ.Core.Communication.Mediator;

public interface IMediatorHandler
{
    Task PublicarEvento<T>(T evento) where T : Event;
    Task<TResponse> EnviarComando<TResponse>(Command<TResponse> comando);
    Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
    //Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent;
}