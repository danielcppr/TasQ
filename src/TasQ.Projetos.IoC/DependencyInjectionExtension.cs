using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TasQ.Core.Communication.Mediator;
using TasQ.Core.Messages.CommonMessages.Notifications;
using TasQ.Projetos.Data;
using TasQ.Projetos.Data.Repositories;
using TasQ.Projetos.Domain.Interfaces;

namespace TasQ.Projetos.IoC;

public static class DependencyInjectionExtension
{
    public static IServiceCollection RegistrarServicos(this IServiceCollection services)
    {
        //Migration Service
        services.AddSingleton<MigrationService>();

        //Mediator
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        //Notificacoes
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        //Projetos
        services.AddScoped<ProjetoDbContext>();
        services.AddScoped<IProjetoRepository, ProjetoRepository>();
        services.AddScoped<IRequestHandler<CriarProjetoCommand, bool>, ProjetoCommandHandler>();

        return services;
    }

    //private static IServiceCollection InfraInjection(this IServiceCollection services) 
    //{
    //    return services; 
    //}

    //private static IServiceCollection RepositoryInjection(this IServiceCollection services) 
    //{ 
    //    return services; 
    //}

    //private static IServiceCollection ServiceInjection(this IServiceCollection services) 
    //{
        
    //    return services; 
    //}
}