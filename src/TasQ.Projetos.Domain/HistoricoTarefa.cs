using TasQ.Core.DomainObjects;

namespace TasQ.Projetos.Domain;

public class HistoricoTarefa : Entity
{
    public Guid UsuarioLogadoId { get; private set; }
    public Guid TarefaId { get; private set; }
    public DateTime DataHora { get; private set; }
    
    // EF Rel.
    public virtual HistoricoItem HistoricoItem { get; protected set; }
    public virtual Tarefa Tarefa { get; protected set; }

    public HistoricoTarefa(Guid usuarioLogadoId, Guid tarefaId, HistoricoItem historicoItem)
    {
        UsuarioLogadoId = usuarioLogadoId;
        DataHora = DateTime.UtcNow;
        TarefaId = tarefaId;
        HistoricoItem = historicoItem;  
    }
    protected HistoricoTarefa() { }
}