using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TasQ.Core.Communication.Mediator;
using TasQ.Core.Messages.CommonMessages.Notifications;
using TasQ.Projetos.Api.Commands;
using TasQ.Projetos.Api.Commands.Handler;
using TasQ.Projetos.Data;
using TasQ.Projetos.Data.Repositories;
using TasQ.Projetos.Domain.Events;
using TasQ.Projetos.Domain.Events.Handler;
using TasQ.Projetos.Domain.Interfaces;
using TasQ.Projetos.Domain.Services;

namespace TasQ.Projetos.Api.Setup;

public static class DependencyInjectionExtension
{
    public static IServiceCollection RegistrarServicos(this IServiceCollection services)
    {
        services.AddSingleton<MigrationService>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<ProjetoDbContext>();

        services.AddScoped<IProjetoRepository, ProjetoRepository>();

        //Commands
        services.AddScoped<IRequestHandler<CriarProjetoCommand, KeyValuePair<bool, Guid>>, ProjetoCommandHandler>();
        services.AddScoped<IRequestHandler<AdicionarTarefaCommand, KeyValuePair<bool, Guid>>, ProjetoCommandHandler>();
        services.AddScoped<IRequestHandler<AdicionarTarefaCommand, KeyValuePair<bool, Guid>>, ProjetoCommandHandler>();
        services.AddScoped<IRequestHandler<AdicionarTarefaCommand, KeyValuePair<bool, Guid>>, ProjetoCommandHandler>();
        services.AddScoped<IRequestHandler<RemoverTarefaCommand, bool>, ProjetoCommandHandler>();
        services.AddScoped<IRequestHandler<RemoverProjetoCommand, bool>, ProjetoCommandHandler>();

        //Notificacoes
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();


        //Eventos
        services.AddScoped<INotificationHandler<TarefaAtualizadaEvent>, ProjetoEventHandler>();


        //Domain Services
        services.AddScoped<IHistoricoTarefaService, HistoricoTarefaService>();


        //Application Services

        return services;
    }
}