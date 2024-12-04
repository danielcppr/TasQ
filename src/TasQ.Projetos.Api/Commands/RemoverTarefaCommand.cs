using FluentValidation;
using TasQ.Core.Messages;

namespace TasQ.Projetos.Api.Commands
{
    public class RemoverTarefaCommand : Command<bool>
    {
        public Guid UsuarioLogadoId { get; set; }
        public Guid TarefaId { get; set; }

        public RemoverTarefaCommand(Guid usuarioLogadoId, Guid tarefaId)
        {
            UsuarioLogadoId = usuarioLogadoId;
            TarefaId = tarefaId;
        }

        public override bool EhValido()
        {
            ValidationResult = new RemoverTarefaCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RemoverTarefaCommandValidation : AbstractValidator<RemoverTarefaCommand>
        {
            public RemoverTarefaCommandValidation()
            {
                
            }
        }
    }
}
