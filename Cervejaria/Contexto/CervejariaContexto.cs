using Cervejaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Contexto
{
    public class CervejariaContexto : DbContext
    {
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Estoque> Estoques { get; set; }
        public virtual DbSet<Receita> Receitas { get; set; }
        public virtual DbSet<Producao> Producoes { get; set; }
        public virtual DbSet<ReceitaIngrediente> ReceitaIngredientes { get; set; }
        public virtual DbSet<Ingrediente> Ingredientes { get; set; }

        public CervejariaContexto() { }

        public CervejariaContexto(DbContextOptions<CervejariaContexto> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("ServerConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                   new Usuario
                   {
                       Id = 1,
                       Nome = "admin",
                       Senha = "root1234",
                       NomeEmpresa = "admin",
                       Cnpj = "11111111000111",
                       Email = "admin@admin.com"
                   },
                    new Usuario
                    {
                        Id = 2,
                        Nome = "Murilo",
                        Senha = "12345678",
                        NomeEmpresa = "kairos",
                        Cnpj = "11111111111112",
                        Email = "murilo@kairos.com"
                    },
                     new Usuario
                     {
                         Id = 3,
                         Nome = "Cintia",
                         Senha = "12345678",
                         NomeEmpresa = "lohn",
                         Cnpj = "11111111111113",
                         Email = "cintia@lohn.com"
                     }
            );
            modelBuilder.Entity<Receita>().HasData(
                new Receita
                    {
                        Id = 1,
                        NomeReceita =  "Pilsen",
                        Responsavel = "Murilo",
                        Estilo = "American Lager",
                        UltimaAtualizacao= new DateTime(2023,06,16),
                        Orcamento= 2985,
                        VolumeReceita= 2000
                    }
                );
            modelBuilder.Entity<Ingrediente>().HasData(
                    new Ingrediente
                    {
                        Id= 1,
                        NomeIngrediente= "Malte Pilsen",
                        IdEstoque= 1,
                        Quantidade= 10000,
                        Tipo= "Malte",
                        ValorUnidade= 2.5,
                        ValorTotal= 25000,
                        Unidade= "kg",
                        Fornecedor= "agraria",
                        Validade= new DateTime(2024,04,01),
                        DataEntrada= new DateTime(2023,04,20)
                    },
                    new Ingrediente
                    {
                        Id = 2,
                        NomeIngrediente = "Lupulo Tradition",
                        IdEstoque = 2,
                        Quantidade = 5000,
                        Tipo = "Lupulo",
                        ValorUnidade = 3.5,
                        ValorTotal = 17500,
                        Unidade = "g",
                        Fornecedor = "agraria",
                        Validade = new DateTime(2024, 04, 01),
                        DataEntrada = new DateTime(2023, 04, 20)
                    },                    
                    new Ingrediente
                    {
                        Id = 3,
                        NomeIngrediente = "Fermento NovaLager",
                        IdEstoque = 3,
                        Quantidade = 2,
                        Tipo = "Fermento",
                        ValorUnidade = 535,
                        ValorTotal = 1070,
                        Unidade = "pc 500g",
                        Fornecedor = "agraria",
                        Validade = new DateTime(2024, 04, 01),
                        DataEntrada = new DateTime(2023, 04, 20)
                    }
                );
        }
    
    }
}
