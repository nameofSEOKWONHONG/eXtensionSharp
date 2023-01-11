using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace eXtensionSharp
{
    public static class XLamda
    {
        private static string xToString(Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Lambda:
                    //x => (Something), return only (Something), the Body
                    return xToString(((LambdaExpression)expr).Body);

                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    //type casts are not important
                    return xToString(((UnaryExpression)expr).Operand);

                case ExpressionType.Call:
                    //method call can be an Indexer (get_Item),
                    var callExpr = (MethodCallExpression)expr;
                    if (callExpr.Method.Name == "get_Item")
                    {
                        //indexer call
                        return xToString(callExpr.Object) + "[" +
                               string.Join(",", callExpr.Arguments.Select(xToString)) + "]";
                    }
                    else
                    {
                        //method call
                        var arguments = callExpr.Arguments.Select(xToString).ToArray();
                        string target;
                        if (callExpr.Method.IsDefined(typeof(ExtensionAttribute), false))
                        {
                            //extension method
                            target = string.Join(".", arguments[0], callExpr.Method.Name);
                            arguments = arguments.Skip(1).ToArray();
                        }
                        else if (callExpr.Object == null)
                        {
                            //static method
                            target = callExpr.Method.Name;
                        }
                        else
                        {
                            //instance method
                            target = string.Join(".", xToString(callExpr.Object), callExpr.Method.Name);
                        }

                        return target + "(" + string.Join(",", arguments) + ")";
                    }
                case ExpressionType.MemberAccess:
                    //property or field access
                    var memberExpr = (MemberExpression)expr;
                    if (memberExpr.Expression.Type.Name.Contains("<>")) //closure type, don't show it.
                        return memberExpr.Member.Name;
                    else
                        return string.Join(".", xToString(memberExpr.Expression), memberExpr.Member.Name);
            }

            //by default, show the standard implementation
            return expr.ToString();
        }
    }
}