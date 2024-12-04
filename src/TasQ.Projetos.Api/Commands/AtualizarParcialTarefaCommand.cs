using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TasQ.Core.Messages;
using TasQ.Projetos.Api.DTOs;
using TasQ.Projetos.Domain;

namespace TasQ.Projetos.Api.Commands;
public class AtualizarParcialTarefaCommand : Command<bool>
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O Id da tarefa deve ser informado.")]
    public Guid TarefaId { get; private init; }
    public AtualizarParcialTarefaRequest AtualizarParcialRequest { get; private init; }

    public AtualizarParcialTarefaCommand(Guid tarefaId, AtualizarParcialTarefaRequest atualizarParcialRequest)
    {
        TarefaId = tarefaId;
        AtualizarParcialRequest = atualizarParcialRequest;
    }

    public override bool EhValido()
    {
        ValidationResult = new AtualizarParcialTarefaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AtualizarParcialTarefaCommandValidation : AbstractValidator<AtualizarParcialTarefaCommand>
    {
        public AtualizarParcialTarefaCommandValidation()
        {
        }
    }
}
