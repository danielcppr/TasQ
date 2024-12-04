using TasQ.Core.DomainObjects;
using TasQ.Core.Messages;
using TasQ.Projetos.Domain;

namespace TasQ.Projetos.Domain.Events;

public class TarefaAtualizadaEvent : Event
{
    public Guid UsuarioLogadoId { get; }
    public Guid TarefaId { get; }
    //public AtualizacaoDadosHistoricoTarefa Dados { get; }
    public string? ValorNovo { get; }
    public TipoAtualizacaoTarefaEnum Tipo { get; }

    //public TarefaAtualizadaEvent(Guid usuarioId, Guid tarefaId, AtualizacaoDadosHistoricoTarefa dados)
    //{
    //    UsuarioLogadoId = usuarioId;
    //    TarefaId = tarefaId;
    //    Dados = dados;
    //    Tipo = TipoAtualizacaoTarefaEnum.ATUALIZACAO_DADOS;
    //}
    public TarefaAtualizadaEvent(Guid usuarioLogadoId, Guid tarefaId, string? valorNovo, TipoAtualizacaoTarefaEnum tipo)
    {
        UsuarioLogadoId = usuarioLogadoId;
        TarefaId = tarefaId;
        ValorNovo = valorNovo;
        Tipo = tipo;
    }
}
