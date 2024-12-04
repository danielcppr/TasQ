using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasQ.Core.Communication.Mediator;
using TasQ.Core.Messages.CommonMessages.Notifications;
using TasQ.Projetos.Api.Commands;
using TasQ.Projetos.Api.Controllers.Config;
using TasQ.Projetos.Api.DTOs;
using TasQ.Projetos.Api.Setup;

namespace TasQ.Projetos.Api.Controllers;

//TODO: Adicionar response padronizado em todas as rotas

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
//[RequireHeaders(["X-Usuario-Logado-Id", "X-Usuario-Logado-Role"])]
public class ProjetoController : ControllerBase
{
    private readonly INotificationHandler<DomainNotification> _notificationHandler;
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjetoController(IMediatorHandler mediatorHandler, INotificationHandler<DomainNotification> notificationHandler, IHttpContextAccessor httpContextAccessor)
    {
        _mediatorHandler = mediatorHandler;
        _notificationHandler = notificationHandler;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("CriarProjeto")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequireHeaders(["X-Usuario-Logado-Id", "X-Usuario-Logado-Role"])]
    public async Task<IActionResult> CriarProjeto(CriarProjetoCommand command)
    {
        try
        {
            var retornoCommand = await _mediatorHandler.EnviarComando(command);
            return retornoCommand.Key ? Created("Id", retornoCommand.Value) : BadRequest();
        }
        catch (Exception e)
        {
            // TODO: Criar ExceptionMiddleware
            return StatusCode(500, "Ops... Ocorreu um erro interno.");
        }
    }


    [HttpPost("AdicionarTarefa")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequireHeaders(["X-Usuario-Logado-Id", "X-Usuario-Logado-Role"])]
    public async Task<IActionResult> AdicionarTarefa(AdicionarTarefaCommand command)
    {
        try
        {
            var retornoCommand = await _mediatorHandler.EnviarComando(command);
            return retornoCommand.Key ? Created("Id", retornoCommand.Value) : BadRequest();
        }
        catch (Exception e)
        {   
            return StatusCode(500, "Ops... Ocorreu um erro interno.");
        }
    }

    [HttpDelete("RemoverTarefa")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequireHeaders(["X-Usuario-Logado-Id", "X-Usuario-Logado-Role"])]
    public async Task<IActionResult> RemoverTarefa([FromQuery] Guid tarefaId)
    {
        try
        {
            if (!_httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("X-Usuario-Logado-Id",
                    out var valorHeader) || !Guid.TryParse(valorHeader, out var usuarioLogadoId))
                return BadRequest();

            var command = new RemoverTarefaCommand(usuarioLogadoId, tarefaId);
            var retornoCommand = await _mediatorHandler.EnviarComando(command);

            return retornoCommand ? Ok() : BadRequest();
        }
        catch (Exception e)
        {
            return StatusCode(500, "Ops... Ocorreu um erro interno.");
        }
    }

    [HttpDelete("RemoverProjeto")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequireHeaders(["X-Usuario-Logado-Id", "X-Usuario-Logado-Role"])]
    public async Task<IActionResult> RemoverProjeto([FromQuery] Guid projetoId)
    {
        try
        {
            if (!_httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("X-Usuario-Logado-Id",
                    out var valorHeader) || !Guid.TryParse(valorHeader, out var usuarioLogadoId))
                return BadRequest();

            var command = new RemoverProjetoCommand(usuarioLogadoId, projetoId);
            var retornoCommand = await _mediatorHandler.EnviarComando(command);

            return retornoCommand ? Ok() : BadRequest();
        }
        catch (Exception e)
        {
            return StatusCode(500, "Ops... Ocorreu um erro interno.");
        }
    }
}
