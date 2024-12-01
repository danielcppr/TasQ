namespace TasQ.Projetos.Domain;

public class HistoricoItem // VO
{
    public string CampoAtualizado { get; private set; }
    public string ValorNovo { get; private set; }
    public string ValorAnterior { get; private set; }

    public HistoricoItem(string campoAtualizado, string valorNovo, string valorAnterior)
    {
        CampoAtualizado = campoAtualizado;
        ValorNovo = valorNovo;
        ValorAnterior = valorAnterior;  
    }
}