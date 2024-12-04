using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FluentValidation;
using TasQ.Core.Messages;

namespace TasQ.Projetos.Api.Commands;

public class CriarProjetoCommand : Command<KeyValuePair<bool, Guid>>
{
    public Guid CriadorUsuarioId { get; }
    public string Titulo { get; }
    public string? Descricao {get; }
    public DateTime DataFinalizacao {get; }
    
    public CriarProjetoCommand(Guid criadorUsuarioId, string titulo, string? descricao, DateTime dataFinalizacao)
    {
        CriadorUsuarioId = criadorUsuarioId;
        Titulo = titulo;
        Descricao = descricao;
        DataFinalizacao = dataFinalizacao;
    }

    public override bool EhValido()
    {
        ValidationResult = new CriarProjetoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class CriarProjetoCommandValidation : AbstractValidator<CriarProjetoCommand>
    {
        public CriarProjetoCommandValidation()
        {
            RuleFor(c => c.CriadorUsuarioId)
                .NotEqual(Guid.Empty)
                .WithName(c => nameof(c.CriadorUsuarioId))
                .WithMessage("O Usuário criador deve ser informado");

            RuleFor(c => c.Titulo)
                .Must(titulo => !string.IsNullOrEmpty(titulo))
                .MinimumLength(6)
                .MaximumLength(30)
                .WithName(c => nameof(c.Titulo))
                .WithMessage("O Título do Projeto deve ser informado e ter entre 6 e 30 caracteres.");

            RuleFor(c => c.Descricao)
                .MaximumLength(1000)
                .When(w => !string.IsNullOrEmpty(w.Descricao))
                .WithName(c => nameof(c.Descricao))
                .WithMessage(
                    "A Descrição do Projeto, caso informada, deve ter no máximo 1000 caracteres");

            RuleFor(c => c.DataFinalizacao)
                .GreaterThan(DateTime.UtcNow)
                .WithName(c => nameof(c.DataFinalizacao))
                .WithMessage("O Data de finalização do Projeto deve ser maior que a data de hoje");
        }
    }
}