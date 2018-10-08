using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Linq
{
    public class SqlVisitor : ExpressionTreeVisitor
    {
        private string sqlStatement = "";
        private string tableName = "";
        private string parameter = "";
        private List<string> columnNames = new List<string>();

        public override Expression Visit(Expression e)
        {
            if (e != null)
            {
                // Console.WriteLine(e.NodeType);
            }
            return base.Visit(e);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            // Console.WriteLine($"  Constant = {c.Value}");

            if (c.Value.GetType().IsGenericType && (c.Value.GetType().GetGenericTypeDefinition() == typeof(DemoLinq<>)))
            {
                tableName = c.Value.GetType().GetGenericArguments().Single().GetCustomAttributes(true).OfType<Table>().FirstOrDefault().Name;

                var tableProperty = Type.GetType(Assembly.GetExecutingAssembly().GetName().Name + "." + c.Value.GetType().GetGenericArguments().Single().Name).GetProperties();

                foreach(var prop in tableProperty)
                {
                    columnNames.Add(prop.Name);
                }
            }

            return base.VisitConstant(c);
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            sqlStatement = ("SELECT " + string.Join(", ", columnNames) + " FROM " + tableName + $" WHERE {Visit(b.Left)} {b.NodeType} {Visit(b.Right)}");
            return b;
            // return base.VisitBinary(b);
        }

        protected override ParameterExpression VisitParameter(ParameterExpression p)
        {
            parameter = p.Name;
            return p;
        }

        public string GetSqlStatement()
        {
            sqlStatement = sqlStatement.Replace(parameter + ".", tableName + ".");
            sqlStatement = sqlStatement.Replace("GreaterThan", ">=");
            sqlStatement = sqlStatement.Replace("Equal", "=");
            sqlStatement = sqlStatement.Replace("AndAlso", "AND");

            return sqlStatement;
        }
    }
}
