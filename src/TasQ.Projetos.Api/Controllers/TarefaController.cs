using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TasQ.Core.Communication.Mediator;
using TasQ.Core.Messages.CommonMessages.Notifications;
using TasQ.Projetos.Api.Commands;
using TasQ.Projetos.Api.DTOs;
using TasQ.Projetos.Api.Setup;
using TasQ.Projetos.Domain.Interfaces;
using TasQ.Projetos.Domain.Services;

namespace TasQ.Projetos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[RequireHeaders(["X-Usuario-Logado-Id", "X-Usuario-Logado-Role"])]
    public class TarefaController : ControllerBase
    {
        private readonly INotificationHandler<DomainNotification> _notificationHandler;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IHistoricoTarefaService _historicoTarefaService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TarefaController(IMediatorHandler mediatorHandler, INotificationHandler<DomainNotification> notificationHandler, IHistoricoTarefaService historicoTarefaService, IHttpContextAccessor httpContextAccessor)
        {
            _mediatorHandler = mediatorHandler;
            _notificationHandler = notificationHandler;
            _historicoTarefaService = historicoTarefaService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPatch("AtualizarParcial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequireHeaders(["X-Usuario-Logado-Id", "X-Usuario-Logado-Role"])]
        public async Task<IActionResult> AtualizarParcial([FromQuery] Guid tarefaId, [FromBody] AtualizarParcialTarefaRequest request)
        {
            try
            {
                var command = new AtualizarParcialTarefaCommand(tarefaId, request);
                var retornoCommand = await _mediatorHandler.EnviarComando(command);
                return retornoCommand ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ops... Ocorreu um erro interno.");
            }
        }

        [HttpPost("AdicionarComentario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequireHeaders(["X-Usuario-Logado-Id", "X-Usuario-Logado-Role"])]
        public async Task<IActionResult> AdicionarComentario([FromQuery] Guid tarefaId,
            [FromBody] ComentarioTarefaRequest request)
        {
            try
            {
                if (!_httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("X-Usuario-Logado-Id",
                        out var valorHeader) || !Guid.TryParse(valorHeader, out var usuarioLogadoId))
                    return BadRequest();

                var sucesso = await _historicoTarefaService.AdicionarComentarioTarefa(usuarioLogadoId, tarefaId, request.Conteudo);

                return sucesso ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ops... Ocorreu um erro interno.");
            }
        }
    }
}
