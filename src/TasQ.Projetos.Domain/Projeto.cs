using FluentValidation;
using FluentValidation.Results;
using TasQ.Core.DomainObjects;

namespace TasQ.Projetos.Domain;

public class Projeto : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public DateTime? PrazoFinalizacao { get; private set; }
    public bool IsFinalizado { get; private set; }

    // EF Relation
    public ICollection<Tarefa> Tarefas { get; set; }
    public ICollection<UsuarioProjeto> UsuariosProjetos { get; set; }

    public Projeto(string nome, 
        string descricao, 
        DateTime? prazoFinalizacao, 
        ICollection<UsuarioProjeto> usuariosProjeto)
    {
        Nome = nome;
        Descricao = descricao;
        PrazoFinalizacao = prazoFinalizacao;
        IsFinalizado = false;
        UsuariosProjetos = usuariosProjeto;
    }

    protected Projeto() { }

    private ValidationResult ValidarSePodeSerExcluido()
    {
        return new ProjetoPodeSerExcluidoValidation().Validate(this);
    }

    private ValidationResult ValidarSePodeAdicionarTarefas(int quantidade)
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
            RuleFor(p => p.Tarefas.Count)
                .InclusiveBetween(0, LIMITE_TAREFAS - qtdade)
                .WithMessage(p => $"Não é possível adicionar mais que {ObtemQuantidadePermitida(p.Tarefas)} tarefas à esse projeto pois ultrapassa o limite máximo de {LIMITE_TAREFAS}.");
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
        }

        protected static bool TarefaFoiFinalizada(Tarefa tarefa) 
            => tarefa.ValidarSeFinalizada();
    }

}