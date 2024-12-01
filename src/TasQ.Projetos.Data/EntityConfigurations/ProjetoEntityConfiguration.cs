using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasQ.Projetos.Domain;

namespace TasQ.Projetos.Data.EntityConfigurations;

public class ProjetoEntityConfiguration : IEntityTypeConfiguration<Projeto>
{
    public void Configure(EntityTypeBuilder<Projeto> builder)
    {
        builder.Property(p => p.Descricao)
            .HasColumnType("varchar(250)");

        builder.HasMany(p => p.Tarefas)
            .WithOne(t => t.Projeto)
            .HasForeignKey(t => t.ProjetoId);

        builder.OwnsMany(
            p => p.UsuariosProjetos, // Coleção de Value Objects
            up =>
            {
                //// Nome da tabela de junção
                //up.ToTable("UsuariosProjetos");

                // Chave composta (sem identidade própria)
                up.HasKey(x => new { x.UsuarioId, x.ProjetoId });

                // Chave estrangeira para Usuario
                up.Property(x => x.UsuarioId)
                    .IsRequired();

                // Relacionamento com a tabela de Usuario
                up.WithOwner() // Value Object pertence a Projeto
                    .HasForeignKey("ProjetoId"); // Chave estrangeira para Projeto
            });


    }
}