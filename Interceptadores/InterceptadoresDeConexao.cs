using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominandoEFCore.Interceptadores
{
    public class InterceptadoresDeConexao : DbConnectionInterceptor
    {
        public override InterceptionResult ConnectionOpening(
            DbConnection connection, 
            ConnectionEventData eventData, 
            InterceptionResult result)
        {
            System.Console.WriteLine("Interceptador de Conexão - ConnectionOpening");
            
            var connectionString =  ((SqlConnection)connection).ConnectionString;

            System.Console.WriteLine($"String de Conexão: {connectionString}");
            
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                ApplicationName ="CursoEFCore"
            };

            connection.ConnectionString = connectionStringBuilder.ToString();

            System.Console.WriteLine($"String de Conexão Atualizada: {connectionStringBuilder.ToString()}");

            return  result;
        }
    }
}
