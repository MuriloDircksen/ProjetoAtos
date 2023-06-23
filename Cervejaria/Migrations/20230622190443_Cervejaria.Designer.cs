﻿// <auto-generated />
using System;
using Cervejaria.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cervejaria.Migrations
{
    [DbContext(typeof(CervejariaContexto))]
    [Migration("20230622190443_Cervejaria")]
    partial class Cervejaria
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cervejaria.Models.Estoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NomeEstoque")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("Estoques");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            NomeEstoque = "Malte"
                        },
                        new
                        {
                            Id = 2,
                            NomeEstoque = "Lupulo"
                        },
                        new
                        {
                            Id = 3,
                            NomeEstoque = "Fermento"
                        },
                        new
                        {
                            Id = 4,
                            NomeEstoque = "Sais"
                        });
                });

            modelBuilder.Entity("Cervejaria.Models.Ingrediente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataEntrada")
                        .HasColumnType("datetime2");

                    b.Property<string>("Fornecedor")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("IdEstoque")
                        .HasColumnType("int");

                    b.Property<string>("NomeIngrediente")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("Quantidade")
                        .HasColumnType("float");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Unidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Validade")
                        .HasColumnType("datetime2");

                    b.Property<double>("ValorTotal")
                        .HasColumnType("float");

                    b.Property<double>("ValorUnidade")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("IdEstoque");

                    b.ToTable("Ingredientes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DataEntrada = new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Fornecedor = "agraria",
                            IdEstoque = 1,
                            NomeIngrediente = "Malte Pilsen",
                            Quantidade = 10000.0,
                            Tipo = "Malte",
                            Unidade = "kg",
                            Validade = new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ValorTotal = 25000.0,
                            ValorUnidade = 2.5
                        },
                        new
                        {
                            Id = 2,
                            DataEntrada = new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Fornecedor = "agraria",
                            IdEstoque = 2,
                            NomeIngrediente = "Lupulo Tradition",
                            Quantidade = 5000.0,
                            Tipo = "Lupulo",
                            Unidade = "g",
                            Validade = new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ValorTotal = 17500.0,
                            ValorUnidade = 3.5
                        },
                        new
                        {
                            Id = 3,
                            DataEntrada = new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Fornecedor = "agraria",
                            IdEstoque = 3,
                            NomeIngrediente = "Fermento NovaLager",
                            Quantidade = 2.0,
                            Tipo = "Fermento",
                            Unidade = "pc 500g",
                            Validade = new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ValorTotal = 1070.0,
                            ValorUnidade = 535.0
                        },
                        new
                        {
                            Id = 4,
                            DataEntrada = new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Fornecedor = "agraria",
                            IdEstoque = 1,
                            NomeIngrediente = "carafa I",
                            Quantidade = 500.0,
                            Tipo = "malte",
                            Unidade = "kg",
                            Validade = new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ValorTotal = 2750.0,
                            ValorUnidade = 5.5
                        });
                });

            modelBuilder.Entity("Cervejaria.Models.Producao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataProducao")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceitaId")
                        .HasColumnType("int");

                    b.Property<string>("Responsavel")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("VolumeApronte")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ReceitaId");

                    b.ToTable("Producoes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DataProducao = new DateTime(2023, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ReceitaId = 1,
                            Responsavel = "Murilo Dircksen",
                            VolumeApronte = 2000.0
                        },
                        new
                        {
                            Id = 2,
                            DataProducao = new DateTime(2023, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ReceitaId = 1,
                            Responsavel = "luke skywalker",
                            VolumeApronte = 1980.0
                        },
                        new
                        {
                            Id = 3,
                            DataProducao = new DateTime(2023, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ReceitaId = 1,
                            Responsavel = "Murilo Dircksen",
                            VolumeApronte = 2130.0
                        });
                });

            modelBuilder.Entity("Cervejaria.Models.Receita", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estilo")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("NomeReceita")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Orcamento")
                        .HasColumnType("float");

                    b.Property<string>("Responsavel")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UltimaAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<double>("VolumeReceita")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Receitas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Estilo = "American Lager",
                            NomeReceita = "Pilsen",
                            Orcamento = 2985.0,
                            Responsavel = "Murilo Dircksen",
                            UltimaAtualizacao = new DateTime(2023, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            VolumeReceita = 2000.0
                        });
                });

            modelBuilder.Entity("Cervejaria.Models.ReceitaIngrediente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdIngrediente")
                        .HasColumnType("int");

                    b.Property<int>("IdReceita")
                        .HasColumnType("int");

                    b.Property<double>("QuantidadeDeIngrediente")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("IdIngrediente");

                    b.HasIndex("IdReceita");

                    b.ToTable("ReceitaIngredientes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IdIngrediente = 1,
                            IdReceita = 1,
                            QuantidadeDeIngrediente = 700.0
                        },
                        new
                        {
                            Id = 2,
                            IdIngrediente = 3,
                            IdReceita = 1,
                            QuantidadeDeIngrediente = 1.0
                        },
                        new
                        {
                            Id = 3,
                            IdIngrediente = 2,
                            IdReceita = 1,
                            QuantidadeDeIngrediente = 200.0
                        });
                });

            modelBuilder.Entity("Cervejaria.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("NomeEmpresa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("Cnpj")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Cnpj = "11111111000111",
                            Email = "admin@admin.com",
                            Nome = "admin admin",
                            NomeEmpresa = "admin ltda.",
                            Senha = "root1234"
                        },
                        new
                        {
                            Id = 2,
                            Cnpj = "11111111111112",
                            Email = "murilo@kairos.com",
                            Nome = "Murilo Dircksen",
                            NomeEmpresa = "Cervejaria kairos",
                            Senha = "12345678"
                        },
                        new
                        {
                            Id = 3,
                            Cnpj = "11111111111113",
                            Email = "cintia@lohn.com",
                            Nome = "Cintia Veronese",
                            NomeEmpresa = "Cervejaria lohn",
                            Senha = "12345678"
                        });
                });

            modelBuilder.Entity("Cervejaria.Models.Ingrediente", b =>
                {
                    b.HasOne("Cervejaria.Models.Estoque", "Estoque")
                        .WithMany("Ingredientes")
                        .HasForeignKey("IdEstoque")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estoque");
                });

            modelBuilder.Entity("Cervejaria.Models.Producao", b =>
                {
                    b.HasOne("Cervejaria.Models.Receita", "Receita")
                        .WithMany("Producao")
                        .HasForeignKey("ReceitaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receita");
                });

            modelBuilder.Entity("Cervejaria.Models.ReceitaIngrediente", b =>
                {
                    b.HasOne("Cervejaria.Models.Ingrediente", "Ingrediente")
                        .WithMany("ReceitaIngredientes")
                        .HasForeignKey("IdIngrediente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cervejaria.Models.Receita", "Receita")
                        .WithMany("ReceitaIngredientes")
                        .HasForeignKey("IdReceita")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingrediente");

                    b.Navigation("Receita");
                });

            modelBuilder.Entity("Cervejaria.Models.Estoque", b =>
                {
                    b.Navigation("Ingredientes");
                });

            modelBuilder.Entity("Cervejaria.Models.Ingrediente", b =>
                {
                    b.Navigation("ReceitaIngredientes");
                });

            modelBuilder.Entity("Cervejaria.Models.Receita", b =>
                {
                    b.Navigation("Producao");

                    b.Navigation("ReceitaIngredientes");
                });
#pragma warning restore 612, 618
        }
    }
}