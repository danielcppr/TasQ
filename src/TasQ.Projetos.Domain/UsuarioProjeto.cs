namespace TasQ.Projetos.Domain;

public class UsuarioProjeto // Tabela de junção fazendo papel de ValueObject - não possui identidade
{
    public Guid UsuarioId { get; private set; }
    public Guid ProjetoId { get; private set; }
    public UsuarioProjeto(Guid usuarioId, Guid projetoId)
    {
        UsuarioId = usuarioId;
        ProjetoId = projetoId;  
    }
}