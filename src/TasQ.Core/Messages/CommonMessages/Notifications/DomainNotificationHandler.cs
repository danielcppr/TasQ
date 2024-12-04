using MediatR;

namespace TasQ.Core.Messages.CommonMessages.Notifications;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>
{
    private List<DomainNotification> _notificacoes = [];

    public Task Handle(DomainNotification notificacao, CancellationToken cancellationToken)
    {
        _notificacoes.Add(notificacao);
        return Task.CompletedTask;
    }

    public virtual List<DomainNotification> ObterNotificacoes() => _notificacoes;
    
    public virtual bool TemNotificacao() => ObterNotificacoes().Count == 0;

    public void Dispose() => _notificacoes = [];
}