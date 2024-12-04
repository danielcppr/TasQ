using ClosedXML.Excel;
using TasQ.Projetos.Domain.Interfaces;

namespace TasQ.Projetos.Api.ApplicationServices;

public class RelatorioService : IRelatorioService
{
    private readonly IProjetoRepository _projetoRepository;

    public RelatorioService(IProjetoRepository projetoRepository)
    {
        _projetoRepository = projetoRepository;
    }


    public async Task ObterRelatorioTarefasConcluidasUsuarios(int filtroDias)
    {
       
    }
}

public class MediaTarefasConcluidasUsuario
{
    public Guid UsuarioId { get; set; }
    public int MediaTarefasConcluidas { get; set; }

    public MediaTarefasConcluidasUsuario(Guid usuarioId, int mediaTarefasConcluidas)
    {
        UsuarioId = usuarioId;
        MediaTarefasConcluidas = mediaTarefasConcluidas;
    }
}