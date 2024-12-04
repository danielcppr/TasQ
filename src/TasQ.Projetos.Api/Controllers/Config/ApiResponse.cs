namespace TasQ.Projetos.Api.Controllers.Config;

public record ApiResponse<T>
{
    public bool Sucesso;
    public T? Dados;
    public List<KeyValuePair<string, string>>? Mensagens;

    public ApiResponse(bool sucesso, T? dados, List<KeyValuePair<string, string>>? mensagens)
    {
        Sucesso = sucesso;
        Dados = dados;
        Mensagens = mensagens;
    }
}