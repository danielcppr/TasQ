using FluentValidation;
using TasQ.Core.Messages;
using static TasQ.Projetos.Api.Commands.RemoverTarefaCommand;

namespace TasQ.Projetos.Api.Commands
{
    public class RemoverProjetoCommand : Command<bool>
    {
        public Guid ProjetoId { get; set; }
        public Guid UsuarioLogadoId { get; set; }

        public RemoverProjetoCommand(Guid projetoId, Guid usuarioLogadoId)
        {
            ProjetoId = projetoId;
            UsuarioLogadoId = usuarioLogadoId;
        }

        public override bool EhValido()
        {
            ValidationResult = new RemoverProjetoCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RemoverProjetoCommandValidation : AbstractValidator<RemoverProjetoCommand>
        {
            public RemoverProjetoCommandValidation()
            {

            }
        }
    }
}
