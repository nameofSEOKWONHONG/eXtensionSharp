using System.ComponentModel.DataAnnotations.Schema;

namespace eXtensionSharp;

/// <summary>
/// 속성 확장
/// </summary>
public static class XAttributeExtensions
{
    /// <summary>
    /// TableAttribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string xGetTableName<T>()
    {
        var attrs = typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
        if (attrs.xIsEmpty()) throw new Exception("T is not TableAttribute");
        
        return (attrs.First() as TableAttribute)?.Name;
    }
    
    /// <summary>
    /// TableAttribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static (string schemaName, string tableName) xGetSchemaAndTableName<T>()
    {
        var attrs = typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
        if (attrs.xIsEmpty()) throw new Exception("T is not TableAttribute");

        return new((attrs.xFirst() as TableAttribute)?.Schema, (attrs.xFirst() as TableAttribute)?.Name);
    }   
}