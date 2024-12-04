using TasQ.Core.DomainObjects;

namespace TasQ.Projetos.Domain;

public class Tarefa : Entity
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public DateTime DataVencimento  { get; private set; }
    public TarefaStatusEnum Status { get; private set; }
    public TarefaPrioridadeEnum Prioridade { get; private init; }
    public Guid UsuarioResponsavelId { get; private set; }
    public Guid ProjetoId { get; private set; }

    // EF Rel.
    public virtual Projeto Projeto { get; protected set; }
    public virtual IReadOnlyCollection<HistoricoTarefa> Historico { get; protected set; }

    private readonly List<KeyValuePair<string, TipoAtualizacaoTarefaEnum>> _propriedadesAtualizaveis =
    [
        new("Titulo", TipoAtualizacaoTarefaEnum.ATUALIZACAO_TITULO),
        new("Descricao", TipoAtualizacaoTarefaEnum.ATUALIZACAO_DESCRICAO),
        new("DataVencimento", TipoAtualizacaoTarefaEnum.ATUALIZACAO_DATA_VENCIMENTO),
        new("Status", TipoAtualizacaoTarefaEnum.ATUALIZACAO_STATUS),
        new("UsuarioResponsavelId", TipoAtualizacaoTarefaEnum.ATUALIZACAO_RESPONSAVEL)
    ];

    public Tarefa(string titulo, 
        string descricao, 
        DateTime dataVencimento, 
        TarefaPrioridadeEnum prioridade, 
        Guid usuarioResponsavelId, 
        Guid projetoId)
    {
        Titulo = titulo;
        Descricao = descricao;
        DataVencimento = dataVencimento;
        Prioridade = prioridade;
        UsuarioResponsavelId = usuarioResponsavelId;
        ProjetoId = projetoId;
    }

    protected Tarefa()
    {
        
    }

    public IEnumerable<string> ListarPropriedadesAtualizaveis()
    {
        var listaValores = new List<string>();
        _propriedadesAtualizaveis.ForEach(i => listaValores.Add(i.Key));

        return listaValores;
    }


    public object? ObterValorPropriedadeDinamico(string nomePropriedade)
    {
        var propertyInfo = GetType()
            .GetProperties()
            .FirstOrDefault(p => string.Equals(p.Name, nomePropriedade, StringComparison.OrdinalIgnoreCase));

        if (propertyInfo is null || !ListarPropriedadesAtualizaveis().Any(p => string.Equals(nomePropriedade, p, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidDomainException($"A propriedade {nomePropriedade} não existe ou não pode ser atualizada.");


        return propertyInfo.GetValue(this);

    }
    public TipoAtualizacaoTarefaEnum AtualizarPropriedadeDinamico(string nomePropriedade, object? valor)
    {
        var propertyInfo = GetType()
            .GetProperties()
            .FirstOrDefault(p => string.Equals(p.Name, nomePropriedade, StringComparison.OrdinalIgnoreCase));

        if (propertyInfo is null || !ListarPropriedadesAtualizaveis().Any(p => string.Equals(nomePropriedade, p, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidDomainException($"A propriedade {nomePropriedade} não existe ou não pode ser atualizada.");

        Validar();

        propertyInfo.SetValue(this, valor);

        return _propriedadesAtualizaveis
            .FirstOrDefault(p => string.Equals(nomePropriedade, p.Key, StringComparison.OrdinalIgnoreCase))
            .Value;
    }

    private void Validar()
    {
        Validar(() => Titulo.Length <= 50, "O Título do Projeto deve ter no máximo 1000 caracteres", true);
        Validar(() => Descricao.Length <= 1000, "A Descricao do Projeto deve ter no máximo 50 caracteres", true);
    }


    //public ValidationResult AvancarStatusTarefa()
    //{
    //    var validacao = Validar(ValidarSeFinalizada, "Não é possível alterar status de tarefa finalizada.");
    //    if (!validacao.IsValid)
    //        return validacao;


    //    switch (Status)
    //    {
    //        case TarefaStatusEnum.PENDENTE:
    //            Status = TarefaStatusEnum.EM_ANDAMENTO;
    //            break;

    //        case TarefaStatusEnum.EM_ANDAMENTO:
    //            Status = TarefaStatusEnum.CONCLUIDA;
    //            break;

    //        case TarefaStatusEnum.CONCLUIDA:
    //        default:
    //            throw new Exception(); // TODO Alterar para DomainException
    //    }

    //    return validacao;
    //}

    //public void AdicionarHistorico(HistoricoTarefa historico)
    //{
    //    Historico.Add(historico);
    //}

    internal bool ValidarSeFinalizada() 
        => Status == TarefaStatusEnum.CONCLUIDA || ExcluidoEm is not null;

}

