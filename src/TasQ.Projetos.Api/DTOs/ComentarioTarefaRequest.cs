using System.ComponentModel.DataAnnotations;

namespace TasQ.Projetos.Api.DTOs;

public class ComentarioTarefaRequest
{
    [Required(ErrorMessage = "Conteudo do comentário deve ser informado.")]
    public string Conteudo { get; set; }

    public ComentarioTarefaRequest(string conteudo)
    {
        Conteudo = conteudo;
    }
}