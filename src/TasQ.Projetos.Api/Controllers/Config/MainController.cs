using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TasQ.Core.Communication.Mediator;
using TasQ.Core.Messages.CommonMessages.Notifications;

namespace TasQ.Projetos.Api.Controllers.Config
{
    [ApiController]
    public abstract class MainController<T> : ControllerBase where T : ApiResponse<object?>
    {
        private readonly DomainNotificationHandler _notificationHandler;
        private readonly IMediatorHandler _mediatorHandler;

        protected MainController(IMediatorHandler mediatorHandler, INotificationHandler<DomainNotification> notificationHandler)
        {
            _mediatorHandler = mediatorHandler;
            _notificationHandler = (DomainNotificationHandler)notificationHandler;
        }

        protected bool OperacaoValida()
        {
            return !_notificationHandler.TemNotificacao();
        }

        protected List<DomainNotification> ObtemNotifications() 
            => _notificationHandler.ObterNotificacoes();

        protected ApiResponse<T> Resposta(T? result)
        {
            var operacaoValida = OperacaoValida();
            List<KeyValuePair<string, string>> mensagens = [];
            return new ApiResponse<T>(operacaoValida, result, mensagens);
        }
    }
}
