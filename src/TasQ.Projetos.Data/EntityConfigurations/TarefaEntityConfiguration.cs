using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasQ.Projetos.Domain;

namespace TasQ.Projetos.Data.EntityConfigurations;
public class TarefaEntityConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.Property(t => t.Descricao)
            .HasColumnType("varchar(1000)");

        builder.Property(t => t.Titulo)
            .HasColumnType("varchar(50)");

        builder.Property(t => t.Prioridade)
            .HasColumnType("smallint");

        builder.HasMany(t => t.Historico) 
            .WithOne(h => h.Tarefa)
            .HasForeignKey(h => h.TarefaId);
    }
}