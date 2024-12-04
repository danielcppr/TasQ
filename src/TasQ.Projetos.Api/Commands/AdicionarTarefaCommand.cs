using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TasQ.Core.Messages;
using TasQ.Projetos.Domain;

namespace TasQ.Projetos.Api.Commands;
public class AdicionarTarefaCommand : Command<KeyValuePair<bool, Guid>>
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O Id do Projeto deve ser informado.")]
    public Guid ProjetoId { get; }
    public Guid CriadorUsuarioId { get; }
    public Guid ResponsavelUsuarioId { get; }
    public string Titulo { get; }
    public string Descricao { get; }
    public DateTime DataVencimento { get; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "A Prioridade deve ser informada")]
    public TarefaPrioridadeEnum Prioridade { get; }

    public AdicionarTarefaCommand(Guid projetoId, 
        Guid criadorUsuarioId, 
        Guid responsavelUsuarioId, 
        string titulo, 
        string descricao, 
        DateTime dataVencimento, 
        TarefaPrioridadeEnum prioridade)
    {
        ProjetoId = projetoId;
        CriadorUsuarioId = criadorUsuarioId;
        ResponsavelUsuarioId = responsavelUsuarioId;
        Titulo = titulo;
        Descricao = descricao;
        DataVencimento = dataVencimento;
        Prioridade = prioridade;
    }

    public override bool EhValido()
    {
        ValidationResult = new AdicionarTarefaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarTarefaCommandValidation : AbstractValidator<AdicionarTarefaCommand>
    {
        public AdicionarTarefaCommandValidation()
        {
            RuleFor(c => c.ProjetoId)
                .NotEqual(Guid.Empty)
                .WithName(c => nameof(c.ProjetoId))
                .WithMessage("O Id do Projeto deve ser informado.");

            RuleFor(c => c.CriadorUsuarioId)
                .NotEqual(Guid.Empty)
                .WithName(c => nameof(c.CriadorUsuarioId))
                .WithMessage("O Id do Usuario criador da tarefa deve ser informado.");

            RuleFor(c => c.ResponsavelUsuarioId)
                .NotEqual(Guid.Empty)
                .WithName(c => nameof(c.ResponsavelUsuarioId))
                .WithMessage("O Id do Usuario responsável pela tarefa deve ser informado.");

            RuleFor(c => c.Titulo)
                .Must(titulo => !string.IsNullOrEmpty(titulo))
                .MinimumLength(6)
                .MaximumLength(30)
                .WithName(c => nameof(c.Titulo))
                .WithMessage("O Título da Tarefa deve ser informado e ter entre 6 e 50 caracteres.");

            RuleFor(c => c.Descricao)
                .MaximumLength(1000)
                .When(w => !string.IsNullOrEmpty(w.Descricao))
                .WithName(c => nameof(c.Descricao))
                .WithMessage(
                    "A Descrição da Tarefa, caso informada, deve ter no máximo 1000 caracteres");

            RuleFor(c => c.DataVencimento)
                .GreaterThan(DateTime.UtcNow)
                .WithName(c => nameof(c.DataVencimento))
                .WithMessage("O Data de vencimento da Tarefa deve ser maior que a data de hoje");

            RuleFor(c => c.Prioridade)
                .NotNull()
                .Must(p => Enum.IsDefined(p.GetType(), p))
                .WithName(c => nameof(c.Prioridade))
                .WithMessage("Prioridade inválida. Informe 0 - Baixa, 1 - Média ou 2 - Alta");
        }
    }
}
