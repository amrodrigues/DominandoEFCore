using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DominandoEFCore.Domain;
using DominandoEFCore.Funcoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
        // public DbSet<Funcao> Funcoes {get;set;}

        public DbSet<Livro> Livros { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=DevIO-02;Integrated Security=true;pooling=true;";

            optionsBuilder
                .UseSqlServer(strConnection)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                //.AddInterceptors(new Interceptadores.InterceptadoresDeComandos())
                //.AddInterceptors(new Interceptadores.InterceptadoresDeConexao())
                //.AddInterceptors(new Interceptadores.InterceptadorPersistencia())
                ;
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

            modelBuilder
           .HasDbFunction(_minhaFuncao)
           .HasName("LEFT")
           .IsBuiltIn();

            modelBuilder
                .HasDbFunction(_letrasMaiusculas)
                .HasName("ConverterParaLetrasMaiusculas")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(_dateDiff)
                .HasName("DATEDIFF")
                .HasTranslation(p =>
                {
                    var argumentos = p.ToList();

                    var contante = (SqlConstantExpression)argumentos[0];
                    argumentos[0] = new SqlFragmentExpression(contante.Value.ToString());

                    return new SqlFunctionExpression("DATEDIFF", argumentos, false, new[] { false, false, false }, typeof(int), null);

                })
                .IsBuiltIn();
        }

        private static MethodInfo _minhaFuncao = typeof(MinhasFuncoes)
               .GetRuntimeMethod("Left", new[] { typeof(string), typeof(int) });

        private static MethodInfo _letrasMaiusculas = typeof(MinhasFuncoes)
                    .GetRuntimeMethod(nameof(MinhasFuncoes.LetrasMaiusculas), new[] { typeof(string) });

        private static MethodInfo _dateDiff = typeof(MinhasFuncoes)
                    .GetRuntimeMethod(nameof(MinhasFuncoes.DateDiff), new[] { typeof(string), typeof(DateTime), typeof(DateTime) });

    }
}