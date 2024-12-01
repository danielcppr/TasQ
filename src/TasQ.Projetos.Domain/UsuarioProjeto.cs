namespace TasQ.Projetos.Domain;

public class UsuarioProjeto //VO
{
    public Guid UsuarioId { get; private set; }
    public Guid ProjetoId { get; private set; }

    public UsuarioProjeto() { }
}