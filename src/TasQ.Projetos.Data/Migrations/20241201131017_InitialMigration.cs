using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasQ.Projetos.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projetos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    descricao = table.Column<string>(type: "varchar(250)", nullable: false),
                    prazo_finalizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_finalizado = table.Column<bool>(type: "boolean", nullable: false),
                    excluido_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projetos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tarefas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    titulo = table.Column<string>(type: "varchar(100)", nullable: false),
                    descricao = table.Column<string>(type: "varchar(250)", nullable: false),
                    data_vencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    projeto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    excluido_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tarefas", x => x.id);
                    table.ForeignKey(
                        name: "fk_tarefas_projetos_projeto_id",
                        column: x => x.projeto_id,
                        principalTable: "projetos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "usuario_projeto",
                columns: table => new
                {
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    projeto_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario_projeto", x => new { x.usuario_id, x.projeto_id });
                    table.ForeignKey(
                        name: "fk_usuario_projeto_projetos_projeto_id",
                        column: x => x.projeto_id,
                        principalTable: "projetos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "historico_tarefa",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tarefa_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CampoAtualizado = table.Column<string>(type: "text", nullable: false),
                    ValorNovo = table.Column<string>(type: "text", nullable: false),
                    ValorAnterior = table.Column<string>(type: "text", nullable: false),
                    excluido_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_historico_tarefa", x => x.id);
                    table.ForeignKey(
                        name: "fk_historico_tarefa_tarefas_tarefa_id",
                        column: x => x.tarefa_id,
                        principalTable: "tarefas",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_historico_tarefa_tarefa_id",
                table: "historico_tarefa",
                column: "tarefa_id");

            migrationBuilder.CreateIndex(
                name: "ix_tarefas_projeto_id",
                table: "tarefas",
                column: "projeto_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuario_projeto_projeto_id",
                table: "usuario_projeto",
                column: "projeto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historico_tarefa");

            migrationBuilder.DropTable(
                name: "usuario_projeto");

            migrationBuilder.DropTable(
                name: "tarefas");

            migrationBuilder.DropTable(
                name: "projetos");
        }
    }
}
