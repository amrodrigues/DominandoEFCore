using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using DominandoEFCore.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //  ApagarCriarBancoDeDados();
            //TesteInteceptacao();
            //TesteInteceptacaoSaveChanges();
            // ComportamentoPadrao();
            FuncaoDefinidaPeloUsuario();
            DateDIFF();
        }

        static void DateDIFF()
        {
            CadastrarLivro();

            using var db = new  DominandoEFCore.Data.ApplicationContext();

            /*var resultado = db
                .Livros
                .Select(p=>  EF.Functions.DateDiffDay(p.CadastradoEm, DateTime.Now));
                */

            var resultado = db
                .Livros
                .Select(p => DominandoEFCore.Funcoes.MinhasFuncoes.DateDiff("DAY", p.CadastradoEm, DateTime.Now));

            foreach (var diff in resultado)
            {
                Console.WriteLine("Dias desde o cadastro do livro:");
                Console.WriteLine(diff);
                Console.ReadKey();
            }
        }

        static void FuncaoDefinidaPeloUsuario()
        {
            CadastrarLivro();

            using var db = new DominandoEFCore.Data.ApplicationContext();

            db.Database.ExecuteSqlRaw(@"
                CREATE FUNCTION ConverterParaLetrasMaiusculas(@dados VARCHAR(100))
                RETURNS VARCHAR(100)
                BEGIN
                    RETURN UPPER(@dados)
                END");


            var resultado = db.Livros.Select(p => DominandoEFCore.Funcoes.MinhasFuncoes.LetrasMaiusculas(p.Titulo));
            foreach (var parteTitulo in resultado)
            {
                Console.WriteLine("Titulo em maiusculas:");
                Console.WriteLine(parteTitulo);
                Console.ReadKey();
            }
        }

        static void FuncaoLEFT()
        {
            CadastrarLivro();

            using var db = new DominandoEFCore.Data.ApplicationContext();

            var resultado = db.Livros.Select(p => DominandoEFCore.Funcoes.MinhasFuncoes.Left(p.Titulo, 10));
            foreach (var parteTitulo in resultado)
            {
                Console.WriteLine("Parte titulo:" + parteTitulo);
            }
        }
        static void ComportamentoPadrao()
        {
            CadastrarLivro();

            using (var db = new DominandoEFCore.Data.ApplicationContext())
            {
                var transacao = db.Database.BeginTransaction();
                try
                {
                    //var livro = db.Livros.AsNoTracking().FirstOrDefault();
                    var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                    livro.Autor = "Ericka";
                    db.SaveChanges();

                   

                    db.Livros.Add(
                      new Livro
                      {
                          Titulo = "ASP.NET Core Enterprise Applications",
                          Autor = "Anna Maria"
                         
                      });
                    db.SaveChanges();
                    transacao.CreateSavepoint("desfazer_apenas_insercao");
                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "Dominando ao EF Core",
                            //Autor = "Anna Maria Rodrigues"
                            Autor = "Anna Maria Rodrigues".PadLeft(16, '*')
                        });
                    db.SaveChanges();

                    transacao.Commit();
                }
                catch (DbUpdateException e)
                {
                    //transacao.Rollback();
                    transacao.RollbackToSavepoint("desfazer_apenas_insercao");
                    if (e.Entries.Count(p=>p.State == EntityState.Added)== e.Entries.Count)
                    {
                        transacao.Commit();     
                    }

                }
            }

        
        }
        static void CadastrarLivro()
        {
            using (var db = new DominandoEFCore.Data.ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "Introdução ao Entity Framework Core",
                        Autor = "Anna Maria"
                    });

                db.SaveChanges();
            }
        }
    }
}


        //static void TesteInteceptacaoSaveChanges()
        //{
        //    using (var db = new DominandoEFCore.Data.ApplicationContext())
        //    {
        //        //var funcao = new Funcao
        //        //{
        //        //    Data1 = DateTime.Now,
        //        //    Data2 = "2021-01-01",
        //        //    Descricao1 = "Teste Interceptador",
        //        //    Descricao2 = "Teste Interceptador"
        //        //};

        //        //db.Funcoes.Add(funcao);

        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();

        //        db.Funcoes.Add(new Funcao
        //        {
        //                Data1 = DateTime.Now,
        //                Data2 = "2021-01-01",
        //                Descricao2 = "Teste Interceptador",
        //            Descricao1 = "Teste Interceptador SaveChanges"
        //        });
        //        db.SaveChanges();
        //    }
        //}

        //static void TesteInteceptacao()
        //{
        //    using (var db = new DominandoEFCore.Data.ApplicationContext())
        //    {

        //        var consulta = db
        //            .Funcoes
        //            .TagWith("Use NOLOCK")
        //            .AsNoTracking()
        //            .OrderBy(f => f.Id)
        //            .FirstOrDefault();

        //        if (consulta != null)
        //        {
        //            Console.WriteLine($"Consulta:{consulta.Descricao1}");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Nenhum registro encontrado.");
        //        }
        //    }

        //}
        //static void ApagarCriarBancoDeDados()
        //    {

        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();

        //        db.Funcoes.AddRange(
        //        new Funcao
        //        {
        //            Data1 = DateTime.Now.AddDays(2),
        //            Data2 = "2021-01-01",
        //            Descricao1 = "Bala 1 ",
        //            Descricao2 = "Bala 2 "
        //        },
        //        new Funcao
        //        {
        //            Data1 = DateTime.Now.AddDays(1),
        //            Data2 = "XX21-01-01",
        //            Descricao1 = "Bola 2",
        //            Descricao2 = "Bola 2"
        //        },
        //        new Funcao
        //        {
        //            Data1 = DateTime.Now.AddDays(1),
        //            Data2 = "XX21-01-01",
        //            Descricao1 = "Tela",
        //            Descricao2 = "Tela"
        //        });

        //        db.SaveChanges();
        //    }


        //class Program
        //{
        //    static void Main(string[] args)
        //    {
        //        //Collations();
        //        //PropagarDados();
        //        //Esquema();
        //        //ConversorDeValor();
        //        //ConversorCustomizado();
        //        //PropriedadesDeSombra();
        //        //TrabalhandoComPropriedadesDeSombra();
        //        //TiposDePropriedades();
        //        //Relacionamento1Para1();
        //       // Relacionamento1ParaMuitos();
        //        //RelacionamentoMuitosParaMuitos();
        //        //CampoDeApoio();
        //        //ExemploTPH();
        //        //PacotesDePropriedades();
        //        Atributos();
        //    }

        //    static void Collations()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();
        //    }

        //    static void PropagarDados()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();

        //        var script = db.Database.GenerateCreateScript();
        //        Console.WriteLine(script);
        //    }

        //    static void Esquema()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();

        //        var script = db.Database.GenerateCreateScript();

        //        Console.WriteLine(script);
        //    }

        //    static void ConversorDeValor() => Esquema();

        //    static void ConversorCustomizado()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();

        //        db.Conversores.Add(
        //            new Conversor
        //            {
        //                Status = Status.Devolvido,
        //                EnderecoIP = IPAddress.Parse("127.0.0.1"),
        //            });

        //        db.SaveChanges();

        //        var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Analise);

        //        var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Devolvido);
        //    }

        //    static void PropriedadesDeSombra()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();
        //    }

        //    static void TrabalhandoComPropriedadesDeSombra()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();

        //        var departamento = new Departamento
        //        {
        //            Descricao = "Departamento Propriedade de Sombra"
        //        };

        //        db.Departamentos.Add(departamento);

        //        //db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

        //        db.SaveChanges();


        //        //var departamentos = db.Departamentos.Where(p => EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now).ToArray();
        //    }

        //    static void TiposDePropriedades()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();

        //        var cliente = new Cliente
        //        {
        //            Nome = "Fulano de tal",
        //            Telefone = "(79) 98888-9999",
        //            Endereco = new Endereco { Bairro = "Centro", Cidade = "Sao Paulo", Estado ="SP" , Logradouro ="Rua Tres" }
        //        };

        //        db.Clientes.Add(cliente);

        //        db.SaveChanges();

        //        var clientes = db.Clientes.AsNoTracking().ToList();

        //        var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };

        //        clientes.ForEach(cli =>
        //        {
        //            var json = System.Text.Json.JsonSerializer.Serialize(cli, options);

        //            Console.WriteLine(json);
        //        });
        //    }

        //    static void Relacionamento1Para1()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();

        //        var estado = new Estado
        //        {
        //            Nome = "Sergipe",
        //            Governador = new Governador { Nome = "Rafael Almeida", Partido ="PT" }
        //        };

        //        db.Estados.Add(estado);

        //        db.SaveChanges();

        //        var estados = db.Estados.AsNoTracking().ToList();

        //        estados.ForEach(est =>
        //        {
        //            Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");
        //        });
        //    }

        //    static void Relacionamento1ParaMuitos()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            db.Database.EnsureDeleted();
        //            db.Database.EnsureCreated();

        //            var estado = new Estado
        //            {
        //                Nome = "Sergipe",
        //                Governador = new Governador { Nome = "Rafael Almeida", Partido = "PT" }
        //            };

        //            estado.Cidades.Add(new Cidade { Nome = "Itabaiana" });

        //            db.Estados.Add(estado);

        //            db.SaveChanges();
        //        }

        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            var estados = db.Estados.ToList();

        //            estados[0].Cidades.Add(new Cidade { Nome = "Aracaju" });

        //            db.SaveChanges();

        //            foreach (var est in db.Estados.Include(p => p.Cidades).AsNoTracking())
        //            {
        //                Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");

        //                foreach (var cidade in est.Cidades)
        //                {
        //                    Console.WriteLine($"\t Cidade: {cidade.Nome}");
        //                }
        //            }
        //        }
        //    }

        //    static void RelacionamentoMuitosParaMuitos()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            db.Database.EnsureDeleted();
        //            db.Database.EnsureCreated();

        //            var ator1 = new Ator { Nome = "Rafael" };
        //            var ator2 = new Ator { Nome = "Pires" };
        //            var ator3 = new Ator { Nome = "Bruno" };

        //            var filme1 = new Filme { Descricao = "A volta dos que não foram" };
        //            var filme2 = new Filme { Descricao = "De volta para o futuro" };
        //            var filme3 = new Filme { Descricao = "Poeira em alto mar filme" };

        //            ator1.Filmes.Add(filme1);
        //            ator1.Filmes.Add(filme2);

        //            ator2.Filmes.Add(filme1);

        //            filme3.Atores.Add(ator1);
        //            filme3.Atores.Add(ator2);
        //            filme3.Atores.Add(ator3);

        //            db.AddRange(ator1, ator2, filme3);

        //            db.SaveChanges();

        //            foreach (var ator in db.Atores.Include(e => e.Filmes))
        //            {
        //                Console.WriteLine($"Ator: {ator.Nome}");

        //                foreach (var filme in ator.Filmes)
        //                {
        //                    Console.WriteLine($"\tFilme: {filme.Descricao}");
        //                }
        //            }
        //        }
        //    }

        //    static void CampoDeApoio()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            db.Database.EnsureDeleted();
        //            db.Database.EnsureCreated();

        //            var documento = new Documento();
        //            documento.SetCPF("12345678933");

        //            db.Documentos.Add(documento);
        //            db.SaveChanges();

        //            foreach (var doc in db.Documentos.AsNoTracking())
        //            {
        //                Console.WriteLine($"CPF -> {doc.GetCPF()}");
        //            }
        //        }
        //    }

        //    static void ExemploTPH()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            db.Database.EnsureDeleted();
        //            db.Database.EnsureCreated();

        //            var pessoa = new Pessoa { Nome = "Fulano de Tal" };

        //            var instrutor = new Instrutor { Nome = "Rafael Almeida", Tecnologia = ".NET", Desde = DateTime.Now };

        //            var aluno = new Aluno { Nome = "Maria Thysbe", Idade = 31, DataContrato = DateTime.Now.AddDays(-1) };

        //            db.AddRange(pessoa, instrutor, aluno);
        //            db.SaveChanges();

        //            var pessoas = db.Pessoas.AsNoTracking().ToArray();
        //            var instrutores = db.Instrutores.AsNoTracking().ToArray();
        //            //var alunos = db.Alunos.AsNoTracking().ToArray();
        //            var alunos = db.Pessoas.OfType<Aluno>().AsNoTracking().ToArray();

        //            Console.WriteLine("Pessoas **************");
        //            foreach (var p in pessoas)
        //            {
        //                Console.WriteLine($"Id: {p.Id} -> {p.Nome}");
        //            }

        //            Console.WriteLine("Instrutores **************");
        //            foreach (var p in instrutores)
        //            {
        //                Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Tecnologia: {p.Tecnologia}, Desde: {p.Desde}");
        //            }

        //            Console.WriteLine("Alunos **************");
        //            foreach (var p in alunos)
        //            {
        //                Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Idade: {p.Idade}, Data do Contrato: {p.DataContrato}");
        //            }
        //        }
        //    }

        //    static void PacotesDePropriedades()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            db.Database.EnsureDeleted();
        //            db.Database.EnsureCreated();

        //            var configuracao = new Dictionary<string, object>
        //            {
        //                ["Chave"] = "SenhaBancoDeDados",
        //                ["Valor"] = Guid.NewGuid().ToString()
        //            };

        //            db.Configuracoes.Add(configuracao);
        //            db.SaveChanges();

        //            var configuracoes = db
        //                .Configuracoes
        //                .AsNoTracking()
        //                .Where(p => p["Chave"] == "SenhaBancoDeDados")
        //                .ToArray();

        //            foreach (var dic in configuracoes)
        //            {
        //                Console.WriteLine($"Chave: {dic["Chave"]} - Valor: {dic["Valor"]}");
        //            }
        //        }
        //    }

        //    static void Atributos()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            db.Database.EnsureDeleted();
        //            db.Database.EnsureCreated();

        //            var script = db.Database.GenerateCreateScript();

        //            Console.WriteLine(script);

        //            db.Atributos.Add(new Atributo
        //            {
        //                Descricao = "Exemplo",
        //                Observacao = "Observacao"

        //            });

        //            db.SaveChanges();
        //        }
        //    }
        //}
        //class Program
        //{
        //    static void Main(string[] args)
        //    {
        //        FuncoesDeDatas();
        //        //FuncaoLike();
        //        //FuncaoDataLength();
        //        //FuncaoProperty();
        //      // FuncaoCollate();
        //    }

        //    static void FuncaoCollate()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {

        //            var consulta1 = db
        //                .Funcoes
        //                .FirstOrDefault(p => EF.Functions.Collate(p.Descricao1, "SQL_Latin1_General_CP1_CS_AS") == "Tela");

        //            var consulta2 = db
        //                .Funcoes
        //                .FirstOrDefault(p => EF.Functions.Collate(p.Descricao1, "SQL_Latin1_General_CP1_CI_AS") == "tela");

        //            Console.WriteLine($"Consulta1: {consulta1?.Descricao1}");

        //            Console.WriteLine($"Consulta2: {consulta2?.Descricao1}");
        //        }
        //    }

        //    static void FuncaoProperty()
        //    {
        //        ApagarCriarBancoDeDados();

        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            var resultado = db
        //                .Funcoes
        //                //.AsNoTracking()
        //                .FirstOrDefault(p => EF.Property<string>(p, "PropriedadeSombra") == "Teste");

        //            var propriedadeSombra = db
        //                .Entry(resultado)
        //                .Property<string>("PropriedadeSombra")
        //                .CurrentValue;

        //            Console.WriteLine("Resultado:");
        //            Console.WriteLine(propriedadeSombra);
        //        }
        //    }

        //    static void FuncaoDataLength()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            var resultado = db
        //                .Funcoes
        //                .AsNoTracking()
        //                .Select(p => new
        //                {
        //                    TotalBytesCampoData = EF.Functions.DataLength(p.Data1),
        //                    TotalBytes1 = EF.Functions.DataLength(p.Descricao1),
        //                    TotalBytes2 = EF.Functions.DataLength(p.Descricao2),
        //                    Total1 = p.Descricao1.Length,
        //                    Total2 = p.Descricao2.Length
        //                })
        //                .FirstOrDefault();

        //            Console.WriteLine("Resultado:");

        //            Console.WriteLine(resultado);
        //        }
        //    }

        //    static void FuncaoLike()
        //    {
        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            var script = db.Database.GenerateCreateScript();

        //            Console.WriteLine(script);

        //            var dados = db
        //                .Funcoes
        //                .AsNoTracking()
        //                //.Where(p=> EF.Functions.Like(p.Descricao1, "Bo%"))
        //                .Where(p => EF.Functions.Like(p.Descricao1, "B[ao]%"))
        //                .Select(p => p.Descricao1)
        //                .ToArray();

        //            Console.WriteLine("Resultado:");
        //            foreach (var descricao in dados)
        //            {
        //                Console.WriteLine(descricao);
        //            }
        //        }
        //    }

        //    static void FuncoesDeDatas()
        //    {
        //        ApagarCriarBancoDeDados();

        //        using (var db = new DominandoEFCore.Data.ApplicationContext())
        //        {
        //            var script = db.Database.GenerateCreateScript();

        //            Console.WriteLine(script);

        //            var dados = db.Funcoes.AsNoTracking().Select(p =>
        //               new
        //               {
        //                   Dias = EF.Functions.DateDiffDay(DateTime.Now, p.Data1),
        //                   Meses = EF.Functions.DateDiffMonth(DateTime.Now, p.Data1),
        //                   Data = EF.Functions.DateFromParts(2021, 1, 2),
        //                   DataValida = EF.Functions.IsDate(p.Data2),
        //               });

        //            foreach (var f in dados)
        //            {
        //                Console.WriteLine(f);
        //            }

        //        }
        //    }

        //    static void ApagarCriarBancoDeDados()
        //    {
        //        using var db = new DominandoEFCore.Data.ApplicationContext();
        //        db.Database.EnsureDeleted();
        //        db.Database.EnsureCreated();

        //        db.Funcoes.AddRange(
        //        new Funcao
        //        {
        //            Data1 = DateTime.Now.AddDays(2),
        //            Data2 = "2021-01-01",
        //            Descricao1 = "Bala 1 ",
        //            Descricao2 = "Bala 2 "
        //        },
        //        new Funcao
        //        {
        //            Data1 = DateTime.Now.AddDays(1),
        //            Data2 = "XX21-01-01",
        //            Descricao1 = "Bola 2",
        //            Descricao2 = "Bola 2"
        //        },
        //        new Funcao
        //        {
        //            Data1 = DateTime.Now.AddDays(1),
        //            Data2 = "XX21-01-01",
        //            Descricao1 = "Tela",
        //            Descricao2 = "Tela"
        //        });

        //        db.SaveChanges();
        //    }
        //}
    
