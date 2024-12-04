
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasQ.Projetos.Domain;

namespace TasQ.Projetos.Data.EntityConfigurations;
public class HistoricoTarefaEntityConfiguration : IEntityTypeConfiguration<HistoricoTarefa>
{
    public void Configure(EntityTypeBuilder<HistoricoTarefa> builder)
    {
        builder.Property(p => p.UsuarioLogadoId)
            .HasColumnName("usuario_logado_id")
            .IsRequired();

        builder.OwnsOne(h => h.HistoricoItem, i =>
        {
            i.Property(p => p.ValorNovo)
                .HasColumnName("valor_novo")
                .IsRequired();

            i.Property(p => p.Tipo)
                .HasColumnName("tipo_atualizacao")
                .HasColumnType("varchar(50)")
                .HasConversion(
                    v => v.ToString(),
                    v => (TipoAtualizacaoTarefaEnum)Enum.Parse(typeof(TipoAtualizacaoTarefaEnum), v))
                .HasMaxLength(50)
                .IsRequired();
        });
    }
}
