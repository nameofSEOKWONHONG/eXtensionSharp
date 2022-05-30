using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace eXtensionSharp;

public static class XDataExtensions
{
    public static T xDbExecute<T>(this IDbConnection connection, Func<IDbConnection, T> dbConnection)
    {
        T result = default;
        if (connection.State != ConnectionState.Closed)
        {
            connection.Close();
        }
            
        try
        {
            connection.Open();
            result = dbConnection(connection);
            return result;
        }
        finally
        {
            connection.Close();
        }
    }

    public static async Task<T> xDbExecuteAsync<T>(this DbConnection connection,
        Func<DbConnection, Task<T>> dbConnection)
    {
        T result = default;
        if (connection.State != ConnectionState.Closed)
        {
            await connection.CloseAsync();
        }
                
        try
        {
            await connection.OpenAsync();
            result = await dbConnection(connection);
            return result;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}