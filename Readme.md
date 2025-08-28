
<p align="center">
  <img alt="Dominando EF Core" src="https://i.imgur.com/your-image-here.png" width="200px">
</p>

<p align="center">
  <img src="https://img.shields.io/static/v1?label=license&message=MIT&color=5965e0&labelColor=121214">
  <img src="https://img.shields.io/static/v1?label=status&message=Em%20desenvolvimento&color=5965e0&labelColor=121214">
</p>

## Sobre

Este é um projeto de estudos focado em aprofundar o conhecimento e a prática com o **Entity Framework Core**. O objetivo é demonstrar diversas funcionalidades avançadas da ferramenta, como mapeamentos, relacionamentos, herança e manipulação de dados.

## Tecnologias e Conceitos

* **C#**
* **Entity Framework Core (EF Core)**
* **SQL Server**
* **Migrations**
* **Data Annotations**
* **Fluent API**
* **Relacionamentos (1-1, 1-N, N-N)**
* **Herança (Table-Per-Hierarchy - TPH)**
* **Conversores de Valor**
* **Propriedades de Sombra (Shadow Properties)**

---

## Estrutura do Projeto

O projeto principal está no arquivo `Program.cs`, onde cada método estático representa um exemplo de uso do EF Core.

**Para executar um exemplo:**
1. Descomente a chamada do método na função `Main`.
2. Certifique-se de que sua string de conexão no `DbContext` esteja configurada.
3. Rode a aplicação.

---

## Funcionalidades Exploradas

* **Mapeamentos:**
  - Configuração de colunas e tabelas com **Data Annotations** e **Fluent API**.
  - Uso de `DatabaseGeneratedOption` para chaves primárias e colunas computadas.

* **Relacionamentos:**
  - Criação de relacionamentos um para um (`Estado` e `Governador`).
  - Criação de relacionamentos um para muitos (`Estado` e `Cidade`).
  - Criação de relacionamentos muitos para muitos (`Ator` e `Filme`).

* **Herança:**
  - Exemplo de **Table-Per-Hierarchy (TPH)** para mapear herança de classes para uma única tabela.

* **Outros:**
  - Implementação de conversores de valor, como o para `IPAddress`.
  - Utilização de propriedades de sombra para armazenar metadados.
  - Uso de **Backing Fields** para encapsular a lógica de acesso a dados.

### Interceptadores

A seção de interceptadores demonstra como você pode estender o comportamento padrão do Entity Framework Core sem alterar o código principal da sua aplicação.

* **Interceptação de Comandos**: Exemplo de um `DbCommandInterceptor` que injeta a cláusula `WITH (NOLOCK)` em todas as consultas `SELECT`, uma técnica comum para leitura de dados sem bloqueio.
* **Interceptação de `SaveChanges`**: Mostra como interceptar a operação de `SaveChanges` para adicionar lógica de negócio, como preencher automaticamente campos de auditoria antes que a entidade seja persistida no banco de dados.

O uso de interceptadores é uma forma poderosa de aplicar lógica global e transversal à sua aplicação de acesso a dados.

---

## Licença

Este projeto está sob a licença **MIT**. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## Criando o projeto:

 - dotnet new console -o CursoEFCore -n DominandoEFCore 

## Pacotes necessarios:

 - Microsoft.EntityFrameworkCore.SqlServer
 - Microsoft.Logging.Console


