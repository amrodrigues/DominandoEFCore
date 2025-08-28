using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DominandoEFCore.Interceptadores
{
    public class InterceptadoresDeComandos : DbCommandInterceptor
    {
       

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command, 
            CommandEventData eventData, 
            InterceptionResult<DbDataReader> result)
        {
            System.Console.WriteLine("[Sync] Entrei dentro do métdo ReadExecuting");
           // return base.ReaderExecuting(command, eventData, result);
           UsarNolock(command);
            return  result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, 
            CommandEventData eventData, 
            InterceptionResult<DbDataReader> result, 
            CancellationToken cancellationToken = default)
        {
            System.Console.WriteLine("[Async] Entrei dentro do métdo ReadExecuting");
            //return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            
            UsarNolock(command);
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        private static void UsarNolock(DbCommand command)
        {
            if (command.CommandText.Contains("-- Use NOLOCK"))
            { 
            if (!command.CommandText.Contains("WITH (NOLOCK)"))
            {
                // Esta regex é mais simples e confiável
                var newCommandText = Regex.Replace(
                    command.CommandText,
                    @"FROM\s+\[?(\w+)\]?\s+AS\s+\[?(\w+)\]?",
                    "FROM [$1] AS [$2] WITH (NOLOCK)",
                    RegexOptions.IgnoreCase
                );

                // Atualiza o comando
                command.CommandText = newCommandText;
            }
            }
        }
    }
}
