using MediatR;
using TasQ.Core.Communication.Mediator;
using TasQ.Core.DomainObjects;
using TasQ.Core.Messages;
using TasQ.Core.Messages.CommonMessages.Notifications;
using TasQ.Projetos.Api.DTOs;
using TasQ.Projetos.Data.Repositories;
using TasQ.Projetos.Domain;
using TasQ.Projetos.Domain.Events;
using TasQ.Projetos.Domain.Interfaces;

namespace TasQ.Projetos.Api.Commands.Handler;

public class ProjetoCommandHandler : 
    IRequestHandler<CriarProjetoCommand, KeyValuePair<bool, Guid>>,
    IRequestHandler<AdicionarTarefaCommand, KeyValuePair<bool, Guid>>,
    IRequestHandler<AtualizarParcialTarefaCommand, bool>,
    IRequestHandler<RemoverTarefaCommand, bool>,
    IRequestHandler<RemoverProjetoCommand, bool>,
    IDisposable
{
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IProjetoRepository _projetoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public ProjetoCommandHandler(IProjetoRepository projetoRepository,
                                 IMediatorHandler mediatorHandler,
                                 IHttpContextAccessor httpContextAccessor)
    {
        _projetoRepository = projetoRepository;
        _mediatorHandler = mediatorHandler;
        _httpContextAccessor = httpContextAccessor;
    }

    private bool ValidarComando<T>(Command<T> message)
    {
        if (message.EhValido()) return true;

        foreach (var error in message.ValidationResult.Errors)
        {
            //_mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
        }

        return false;
    }

    public async Task<KeyValuePair<bool, Guid>> Handle(CriarProjetoCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request)) return new KeyValuePair<bool, Guid>(false, Guid.Empty);

        var projeto = new Projeto(request.CriadorUsuarioId, request.Titulo, request.Descricao, request.DataFinalizacao);
        
        await _projetoRepository.AdicionarAsync(projeto);

        return await _projetoRepository.UnitOfWork.CommitAsync()
            ? new KeyValuePair<bool, Guid>(true, projeto.Id)
            : new KeyValuePair<bool, Guid>(false, Guid.Empty);
    }

    public async Task<KeyValuePair<bool, Guid>> Handle(AdicionarTarefaCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _projetoRepository.ObterPorId(request.ProjetoId);
        if (!ValidarComando(request)) return new KeyValuePair<bool, Guid>(false, Guid.Empty);

        // TODO: emitir notificações abaixo
        if (projeto.EhNullOuRemovido())
            return new(false, Guid.Empty);
            //throw new InvalidDomainException("Projeto inexistente.");

        if (projeto!.PrazoFinalizacao < request.DataVencimento)
            return new(false, Guid.Empty);
        //throw new InvalidDomainException("Data de Vencimento da tarefa não pode ser menor que o prazo de finalização do Projeto");

        if (!projeto!.ValidarSePodeAdicionarTarefas(1).IsValid)
            return new(false, Guid.Empty);
        //throw new InvalidDomainException("Não é possível adicionar tarefas ao projeto");

        var tarefaNova = Projeto.Factory.CriarTarefaInicial(request.Titulo, request.Descricao,
            request.DataVencimento, request.Prioridade, request.CriadorUsuarioId, request.ProjetoId);
        
        await _projetoRepository.AdicionarAsync(tarefaNova);

        return await _projetoRepository.UnitOfWork.CommitAsync()
            ? new KeyValuePair<bool, Guid>(true, tarefaNova.Id)
            : new KeyValuePair<bool, Guid>(false, Guid.Empty);
    }

    public async Task<bool> Handle(AtualizarParcialTarefaCommand request, CancellationToken cancellationToken)
    {
        var existeErro = false;

        var tarefa = await _projetoRepository.ObterTarefaPorId(request.TarefaId);
        
        if (tarefa.EhNullOuRemovido())
            throw new InvalidDomainException("Tarefa inexistente.");

        // Todo: Notificar
        // Todo: Acho que é melhor deixar essa responsabilidade de acessar o context lá na controller
        // Todo: ou criar um middleware e definir em valor em instancia global?
        if (!_httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("X-Usuario-Logado-Id",
                out var valorHeader) || !Guid.TryParse(valorHeader, out var usuarioLogadoId))
            return false;

        if (!request.AtualizarParcialRequest.CamposAtualizar.All(c =>
                tarefa.ListarPropriedadesAtualizaveis().Any(p => string.Equals(p, c, StringComparison.OrdinalIgnoreCase))))
        {
            // Todo: Lançar notificação

            return false;
        }

        foreach (var campo in request.AtualizarParcialRequest.CamposAtualizar)
        {
            var propertyInfoDto = request.AtualizarParcialRequest.Dados
                .GetType()
                .GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, campo, StringComparison.OrdinalIgnoreCase));

            var propertyInfoCommand = request.AtualizarParcialRequest.Dados
                .GetType()
                .GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, campo, StringComparison.OrdinalIgnoreCase));

            if (propertyInfoDto == null || propertyInfoCommand == null)
            {
                // Todo: add notificacao
                existeErro = true;
                continue;
            //($"Campo informado {campo} é inválido.");
            }

            var novoValor = propertyInfoDto.GetValue(request.AtualizarParcialRequest.Dados);
            var valorAnterior = tarefa.ObterValorPropriedadeDinamico(campo);

            //propertyInfoCommand.GetValue(tarefa);

            if (novoValor == null &&
                Nullable.GetUnderlyingType(propertyInfoCommand.PropertyType) == null)
            {
                // Todo: add notificacao
                existeErro = true;
                continue;
            //($"Campo {campo} não pode ter seu valor removido.");
            }

            var tipoHistorico = tarefa.AtualizarPropriedadeDinamico(campo, novoValor);
            tarefa.AdicionarEvento(new TarefaAtualizadaEvent(usuarioLogadoId, tarefa.Id, novoValor?.ToString(), tipoHistorico) );
        }

        if (!existeErro)
            await _projetoRepository.Atualizar(tarefa);

        return !existeErro && await _projetoRepository.UnitOfWork.CommitAsync();
    }

    public async Task<bool> Handle(RemoverTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _projetoRepository.ObterTarefaPorId(request.TarefaId);
        if (tarefa.EhNullOuRemovido()) return false; // todo: notificar

        tarefa!.ExecutarSoftDelete();

        // TODO: Criar Evento de TarefaRemovida
        return await _projetoRepository.UnitOfWork.CommitAsync();
    }

    public async Task<bool> Handle(RemoverProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _projetoRepository.ObterPorId(request.ProjetoId);
        if (projeto.EhNullOuRemovido()) return false; // todo: notificar

        if (!projeto.ExcluirProjeto().IsValid) return false;
            
        return await _projetoRepository.UnitOfWork.CommitAsync();
    }

    public void Dispose() => _projetoRepository.Dispose();
}
