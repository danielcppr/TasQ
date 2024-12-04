using TasQ.Projetos.Domain;

namespace TasQ.Projetos.Api.DTOs;

public sealed class AtualizarParcialTarefaDados
{
    public Guid UsuarioId { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public DateTime? DataVencimento { get; set; }
    public TarefaStatusEnum? Status { get; set; }
}