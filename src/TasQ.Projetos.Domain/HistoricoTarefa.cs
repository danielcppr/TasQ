using TasQ.Core.DomainObjects;

namespace TasQ.Projetos.Domain;

public class HistoricoTarefa : Entity
{
    public Guid UsuarioId { get; }
    public DateTime Data { get; }
    public Guid TarefaId { get; }
    public HistoricoItem HistoricoItem { get; }
    
    // EF Rel.
    public Tarefa Tarefa { get; set; }

    public HistoricoTarefa(Guid usuarioId, Guid tarefaId, HistoricoItem historicoItem)
    {
        UsuarioId = usuarioId;
        Data = DateTime.Now;
        TarefaId = tarefaId;
        HistoricoItem = historicoItem;  
    }
    protected HistoricoTarefa() { }
}