using FluentValidation;
using FluentValidation.Results;
using TasQ.Core.DomainObjects;

namespace TasQ.Projetos.Domain;

public class Projeto : Entity, IAggregateRoot
{
    public string Titulo { get; private set; }
    public string? Descricao { get; private set; }
    public DateTime? PrazoFinalizacao { get; private set; }
    public bool IsFinalizado { get; private set; }

    // EF Relation
    public virtual ICollection<Tarefa> Tarefas { get; protected set; } = [];
    public virtual ICollection<UsuarioProjeto> UsuariosProjetos { get; protected set; } = [];

    public Projeto(Guid usuarioCriadorId, string titulo, 
        string? descricao, 
        DateTime? prazoFinalizacao)
    {
        Titulo = titulo;
        Descricao = descricao;
        PrazoFinalizacao = prazoFinalizacao;
        IsFinalizado = false;
        UsuariosProjetos = [Factory.CriarUsuarioProjetoItem(usuarioCriadorId, Id)];
    }

    protected Projeto() { }

    private ValidationResult ValidarSePodeSerExcluido()
    {
        return new ProjetoPodeSerExcluidoValidation().Validate(this);
    }

    public ValidationResult ValidarSePodeAdicionarTarefas(int quantidade)
    {
        return new ProjetoPermiteNovasTarefasValidation(quantidade).Validate(this);
    }

    public ValidationResult ExcluirProjeto()
    {
        var validationResult = ValidarSePodeSerExcluido();

        if (!validationResult.IsValid)
            return validationResult;

        ExecutarSoftDelete();

        return validationResult;
    }

    public ValidationResult AdicionarTarefa(Tarefa tarefa)
    {
        var validationResult = ValidarSePodeAdicionarTarefas(1);

        if (!validationResult.IsValid)
            return validationResult;

        Tarefas.Add(tarefa);

        return validationResult;
    }

    public ValidationResult AdicionarTarefasEmLote(IList<Tarefa> tarefas)
    {
        var validationResult = ValidarSePodeAdicionarTarefas(tarefas.Count);
        
        if (!validationResult.IsValid)
            return validationResult;

        foreach (var tarefa in tarefas)
            Tarefas.Add(tarefa);

        return validationResult;
    }

    protected void AdicionarUsuarioProjeto(UsuarioProjeto usuarioProjeto)
    {
        UsuariosProjetos.Add(usuarioProjeto);
    }

    public ValidationResult RemoverTarefa(Tarefa tarefa)
    {
        // TODO: Transformar em DomainException - Não deveria ter chegado até o domínio um projeto que não faz parte.
        var resultadoValidacao = Validar(() => TarefaExisteNoProjeto(tarefa),
            "Tarefa não faz parte do Projeto, logo, não é possível removê-la");

        if (!resultadoValidacao.IsValid)
            return resultadoValidacao;

        Tarefas.Remove(tarefa);

        return resultadoValidacao;
    }

    private bool TarefaExisteNoProjeto(Tarefa tarefa) => Tarefas.Any(t => t.Id == tarefa.Id);


    public class ProjetoPermiteNovasTarefasValidation : AbstractValidator<Projeto>
    {
        private const int LIMITE_TAREFAS = 20;
        public ProjetoPermiteNovasTarefasValidation(int qtdade)
        {
            RuleFor(p => p.Tarefas.Count(t => !t.EhNullOuRemovido()))
                .InclusiveBetween(0, LIMITE_TAREFAS - qtdade)
                .WithMessage(p => $"Não é possível adicionar mais que {ObtemQuantidadePermitida(p.Tarefas)} tarefas à esse projeto pois ultrapassa o limite máximo de {LIMITE_TAREFAS}.");

            RuleFor(p => p.IsFinalizado)
                .Equal(false)
                .WithMessage("Não é permitido adicionar tarefas a um projeto finalizado.");
        }

        protected static int ObtemQuantidadePermitida(ICollection<Tarefa> tarefas) => LIMITE_TAREFAS - tarefas.Count;
    }

    public class ProjetoPodeSerExcluidoValidation : AbstractValidator<Projeto>
    {

        public ProjetoPodeSerExcluidoValidation()
        {
            RuleForEach(p => p.Tarefas)
                .Must(TarefaFoiFinalizada)
                .WithMessage((m, t) => $"A Tarefa {t.Id} está com Status '{t.Status.ToString()}'. Altere o status para Concluído ou remova-a para excluir o Projeto.");

            RuleFor(p => p.Tarefas.Where(t => !t.ExcluidoEm.HasValue))
                .Must(t => !t.Any())
                .WithMessage("Remova todas as tarefas relacionadas ao Projeto.");
        }

        protected static bool TarefaFoiFinalizada(Tarefa tarefa) 
            => tarefa.ValidarSeFinalizada();
    }


    public class Factory
    {
        public static UsuarioProjeto CriarUsuarioProjetoItem(Guid usuarioId, Guid projetoId) =>
            new UsuarioProjeto(usuarioId, projetoId);

        public static Tarefa CriarTarefaInicial(string titulo, string descricao, DateTime dataVencimento,
            TarefaPrioridadeEnum prioridade, Guid usuarioId, Guid projetoId)
            => new Tarefa(titulo, descricao, dataVencimento, prioridade, usuarioId, projetoId);
    }

}