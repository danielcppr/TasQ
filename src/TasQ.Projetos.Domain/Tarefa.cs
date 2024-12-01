using FluentValidation.Results;
using TasQ.Core.DomainObjects;

namespace TasQ.Projetos.Domain;

public class Tarefa : Entity
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public DateTime? DataVencimento  { get; private set; }
    public TarefaStatusEnum Status { get; private set; }
    public TarefaPrioridadeEnum Prioridade { get; }
    public Guid UsuarioId { get; private set; }
    public Guid ProjetoId { get; private set; }

    // EF Rel.
    public Projeto Projeto { get; set; }
    public ICollection<HistoricoTarefa> Historico { get; set; }

    public Tarefa(string titulo, 
        string descricao, 
        DateTime? dataVencimento, 
        TarefaPrioridadeEnum prioridade, 
        Guid usuarioId, 
        Guid projetoId)
    {
        Titulo = titulo;
        Descricao = descricao;
        DataVencimento = dataVencimento;
        Prioridade = prioridade;
        UsuarioId = usuarioId;
        ProjetoId = projetoId;
    }

    protected Tarefa()
    {
        
    }


    public ValidationResult AvancarStatusTarefa()
    {
        var validacao = Validar(ValidarSeFinalizada, "Não é possível alterar status de tarefa finalizada.");
        if (!validacao.IsValid)
            return validacao;


        switch (Status)
        {
            case TarefaStatusEnum.PENDENTE:
                Status = TarefaStatusEnum.EM_ANDAMENTO;
                break;

            case TarefaStatusEnum.EM_ANDAMENTO:
                Status = TarefaStatusEnum.CONCLUIDA;
                break;

            case TarefaStatusEnum.CONCLUIDA:
            default:
                throw new Exception(); // TODO Alterar para DomainException
        }

        return validacao;
    }

    public void AdicionarHistorico(HistoricoTarefa historico)
    {
        Historico.Add(historico);
    }

    internal bool ValidarSeFinalizada() 
        => Status == TarefaStatusEnum.CONCLUIDA || ExcluidoEm is not null;
}