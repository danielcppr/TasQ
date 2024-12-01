
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasQ.Projetos.Domain;

namespace TasQ.Projetos.Data.EntityConfigurations;
public class HistoricoTarefaEntityConfiguration : IEntityTypeConfiguration<HistoricoTarefa>
{
    public void Configure(EntityTypeBuilder<HistoricoTarefa> builder)
    {
        builder.HasOne(h => h.Tarefa)
            .WithMany(t => t.Historico)
            .HasForeignKey(h => h.TarefaId);

        builder.OwnsOne(h => h.HistoricoItem, i =>
        {
            i.Property(p => p.CampoAtualizado)
                .HasColumnName("CampoAtualizado");
            i.Property(p => p.ValorAnterior)
                .HasColumnName("ValorAnterior");
            i.Property(p => p.ValorNovo)
                .HasColumnName("ValorNovo");
        });

        //builder.HasMany(t => t.Historico)
        //    .WithOne(h => h.Tarefa)
        //    .HasForeignKey(h => h.TarefaId);

    }
}
