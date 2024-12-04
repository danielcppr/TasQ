using System.ComponentModel.DataAnnotations.Schema;

namespace TasQ.Projetos.Domain;

public class HistoricoItem // ValueObject
{
    //public Guid UsuarioLogadoId { get; }
    //public ITipoAtualizacaoTarefa Dados { get; set; }
    public TipoAtualizacaoTarefaEnum Tipo { get; }
    public string? ValorNovo { get; }

    //public HistoricoItem(ITipoAtualizacaoTarefa dados, TipoAtualizacaoTarefaEnum tipo)
    //{
    //    Dados = dados;
    //    Tipo = tipo;
    //}

    public HistoricoItem(TipoAtualizacaoTarefaEnum tipo, string? valorNovo)
    {
        //UsuarioLogadoId = usuarioLogadoId;
        Tipo = tipo;
        ValorNovo = valorNovo;
    }
}

public enum TipoAtualizacaoTarefaEnum
{
    CRIACAO,
    REMOCAO,
    ATUALIZACAO_RESPONSAVEL,
    ATUALIZACAO_TITULO,
    ATUALIZACAO_DESCRICAO,
    ATUALIZACAO_STATUS,
    ATUALIZACAO_DATA_VENCIMENTO,
    COMENTARIO
}

//public class AtualizacaoDadosHistoricoTarefa : ITipoAtualizacaoTarefa
//{
//    public string CampoAtualizado { get; private set; }
//    public string ValorNovo { get; private set; }
//    public string ValorAnterior { get; private set; }

//    public AtualizacaoDadosHistoricoTarefa(string campoAtualizado, string valorNovo, string valorAnterior)
//    {
//        CampoAtualizado = campoAtualizado;
//        ValorNovo = valorNovo;
//        ValorAnterior = valorAnterior;
//    }
//}

public class ComentarioHistoricoTarefa : ITipoAtualizacaoTarefa
{
    //public string Comentario { get; private set; }

    //public ComentarioHistoricoTarefa(string comentario)
    //{
    //    Comentario = comentario;
    //}
}

public interface ITipoAtualizacaoTarefa { }