using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        //public DbSet<Departamento> Departamentos { get; set; }
        //public DbSet<Funcionario> Funcionarios { get; set; }
        //public DbSet<Estado> Estados { get; set; }
        //public DbSet<Conversor> Conversores { get; set; }
        //public DbSet<Cliente> Clientes { get; set; }

        //public DbSet<Ator> Atores { get; set; }
        //public DbSet<Filme> Filmes { get; set; }

        //public DbSet<Documento> Documentos { get; set; }


        //public DbSet<Pessoa> Pessoas { get; set; }
        //public DbSet<Instrutor> Instrutores { get; set; }
        //public DbSet<Aluno> Alunos { get; set; }

        //public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Configuracoes");

        //public DbSet<Atributo> Atributos { get; set; }

        //public DbSet<Aeroporto> Aeroportos { get; set; }
        public DbSet<Funcao> Funcoes {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=DevIO-02;Integrated Security=true;pooling=true;";

            optionsBuilder
                .UseSqlServer(strConnection)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .AddInterceptors(new Interceptadores.InterceptadoresDeComandos())
                .AddInterceptors(new Interceptadores.InterceptadoresDeConexao())
                .AddInterceptors(new Interceptadores.InterceptadorPersistencia());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", b =>
            {
                b.Property<int>("Id");

                b.Property<string>("Chave")
                    .HasColumnType("VARCHAR(40)")
                    .IsRequired();

                b.Property<string>("Valor")
                    .HasColumnType("VARCHAR(255)")
                    .IsRequired();
            });
            //modelBuilder
            //    .Entity<Funcao>(conf=>
            //    {
            //        conf.Property<string>("PropriedadeSombra")
            //            .HasColumnType("VARCHAR(100)")
            //            .HasDefaultValueSql("'Teste'");
            //    });
        }
    }
}