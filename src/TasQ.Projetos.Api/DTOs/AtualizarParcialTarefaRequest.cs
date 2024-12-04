using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TasQ.Projetos.Api.Commands;

namespace TasQ.Projetos.Api.DTOs
{
    public sealed class AtualizarParcialTarefaRequest
    {
        [Required(ErrorMessage = "Informe os dados da tarefa.")]
        public required AtualizarParcialTarefaDados Dados { get; set; }

        [Required(ErrorMessage = "Informe o nome do(s) campo(s) para atualizar.")]
        public required string[] CamposAtualizar { get; set; }
    }
}
