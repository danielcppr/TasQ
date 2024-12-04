using MediatR;
using Microsoft.AspNetCore.Mvc;
using TasQ.Core.Communication.Mediator;
using TasQ.Core.Messages.CommonMessages.Notifications;
using TasQ.Projetos.Api.Setup;

namespace TasQ.Projetos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class RelatorioController : ControllerBase 
{
    private readonly INotificationHandler<DomainNotification> _notificationHandler;
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RelatorioController(IMediatorHandler mediatorHandler, INotificationHandler<DomainNotification> notificationHandler, IHttpContextAccessor httpContextAccessor)
    {
        _mediatorHandler = mediatorHandler;
        _notificationHandler = notificationHandler;
        _httpContextAccessor = httpContextAccessor;
    }
}
