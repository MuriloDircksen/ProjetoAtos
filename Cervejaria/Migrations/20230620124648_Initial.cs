using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cervejaria.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estoques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeEstoque = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receitas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeReceita = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Estilo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Orcamento = table.Column<double>(type: "float", nullable: false),
                    VolumeReceita = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receitas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NomeEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeIngrediente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdEstoque = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<double>(type: "float", nullable: false),
                    ValorUnidade = table.Column<double>(type: "float", nullable: false),
                    ValorTotal = table.Column<double>(type: "float", nullable: false),
                    Unidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fornecedor = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Validade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredientes_Estoques_IdEstoque",
                        column: x => x.IdEstoque,
                        principalTable: "Estoques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceitaId = table.Column<int>(type: "int", nullable: false),
                    VolumeApronte = table.Column<double>(type: "float", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DataProducao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producoes_Receitas_ReceitaId",
                        column: x => x.ReceitaId,
                        principalTable: "Receitas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceitaIngredientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdReceita = table.Column<int>(type: "int", nullable: false),
                    IdIngrediente = table.Column<int>(type: "int", nullable: false),
                    QuantidadeDeIngrediente = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceitaIngredientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceitaIngredientes_Ingredientes_IdIngrediente",
                        column: x => x.IdIngrediente,
                        principalTable: "Ingredientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceitaIngredientes_Receitas_IdReceita",
                        column: x => x.IdReceita,
                        principalTable: "Receitas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Estoques",
                columns: new[] { "Id", "NomeEstoque" },
                values: new object[,]
                {
                    { 1, "Malte" },
                    { 2, "Lupulo" },
                    { 3, "Fermento" },
                    { 4, "Sais" }
                });

            migrationBuilder.InsertData(
                table: "Receitas",
                columns: new[] { "Id", "Estilo", "NomeReceita", "Orcamento", "Responsavel", "UltimaAtualizacao", "VolumeReceita" },
                values: new object[] { 1, "American Lager", "Pilsen", 2985.0, "Murilo", new DateTime(2023, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000.0 });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Cnpj", "Email", "Nome", "NomeEmpresa", "Senha" },
                values: new object[,]
                {
                    { 1, "11111111000111", "admin@admin.com", "admin", "admin", "root1234" },
                    { 2, "11111111111112", "murilo@kairos.com", "Murilo", "kairos", "12345678" },
                    { 3, "11111111111113", "cintia@lohn.com", "Cintia", "lohn", "12345678" }
                });

            migrationBuilder.InsertData(
                table: "Ingredientes",
                columns: new[] { "Id", "DataEntrada", "Fornecedor", "IdEstoque", "NomeIngrediente", "Quantidade", "Tipo", "Unidade", "Validade", "ValorTotal", "ValorUnidade" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "agraria", 1, "Malte Pilsen", 10000.0, "Malte", "kg", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25000.0, 2.5 },
                    { 2, new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "agraria", 2, "Lupulo Tradition", 5000.0, "Lupulo", "g", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17500.0, 3.5 },
                    { 3, new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "agraria", 3, "Fermento NovaLager", 2.0, "Fermento", "pc 500g", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1070.0, 535.0 },
                    { 4, new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "agraria", 1, "carafa I", 500.0, "malte", "kg", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2750.0, 5.5 }
                });

            migrationBuilder.InsertData(
                table: "Producoes",
                columns: new[] { "Id", "DataProducao", "ReceitaId", "Responsavel", "VolumeApronte" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Murilo", 2000.0 },
                    { 2, new DateTime(2023, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Luke", 1980.0 },
                    { 3, new DateTime(2023, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Murilo", 2130.0 }
                });

            migrationBuilder.InsertData(
                table: "ReceitaIngredientes",
                columns: new[] { "Id", "IdIngrediente", "IdReceita", "QuantidadeDeIngrediente" },
                values: new object[,]
                {
                    { 1, 1, 1, 700.0 },
                    { 2, 3, 1, 1.0 },
                    { 3, 2, 1, 200.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredientes_IdEstoque",
                table: "Ingredientes",
                column: "IdEstoque");

            migrationBuilder.CreateIndex(
                name: "IX_Producoes_ReceitaId",
                table: "Producoes",
                column: "ReceitaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceitaIngredientes_IdIngrediente",
                table: "ReceitaIngredientes",
                column: "IdIngrediente");

            migrationBuilder.CreateIndex(
                name: "IX_ReceitaIngredientes_IdReceita",
                table: "ReceitaIngredientes",
                column: "IdReceita");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Cnpj",
                table: "Usuarios",
                column: "Cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producoes");

            migrationBuilder.DropTable(
                name: "ReceitaIngredientes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Ingredientes");

            migrationBuilder.DropTable(
                name: "Receitas");

            migrationBuilder.DropTable(
                name: "Estoques");
        }
    }
}
