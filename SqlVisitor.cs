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
        private List<string> columnNames = new List<string>();

        public string GetNodeType(BinaryExpression b)
        {
            string nodeType = "";
            switch (b.NodeType)
            {
                case ExpressionType.AndAlso:
                    nodeType = "AND";
                    break;
                case ExpressionType.Equal:
                    nodeType = "=";
                    break;
                case ExpressionType.GreaterThan:
                    nodeType = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    nodeType = ">=";
                    break;
                case ExpressionType.IsFalse:
                    nodeType = "IS FALSE";
                    break;
                case ExpressionType.IsTrue:
                    nodeType = "IS TRUE";
                    break;
                case ExpressionType.LessThan:
                    nodeType = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    nodeType = "<=";
                    break;
                case ExpressionType.Modulo:
                    nodeType = "%";
                    break;
                case ExpressionType.Not:
                    nodeType = "!";
                    break;
                case ExpressionType.NotEqual:
                    nodeType = "!=";
                    break;
                case ExpressionType.OrElse:
                    nodeType = "OR";
                    break;
                default:
                    break;
            }
            return nodeType;
        }

        public string GetSqlStatement()
        {
            return $"SELECT {string.Join(", ", columnNames)} FROM {tableName}{sqlStatement}";
        }

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

                foreach (var prop in tableProperty)
                {
                    columnNames.Add(prop.Name);
                }
            }
            else
            {
                sqlStatement += c.Value.GetType() == typeof(string) ? $" '{c.Value}'" : $" {c.Value}";
            }

            return base.VisitConstant(c);
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            sqlStatement += sqlStatement.EndsWith("(") ? "(" : " (";
            Visit(b.Left);
            sqlStatement += $" {GetNodeType(b)}";
            Visit(b.Right);
            sqlStatement += ")";
            // sqlStatement = ("SELECT " + string.Join(", ", columnNames) + " FROM " + tableName + $" WHERE {Visit(b.Left)} {nodeType} {Visit(b.Right)}");

            return b;
            // return base.VisitBinary(b);
        }

        protected override Expression VisitLambda(LambdaExpression lambda)
        {
            sqlStatement += string.IsNullOrEmpty(sqlStatement) ? " WHERE" : " AND";
            return base.VisitLambda(lambda);
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            sqlStatement += m.Member.Name;

            return base.VisitMemberAccess(m);
        }

        //protected override ParameterExpression VisitParameter(ParameterExpression p)
        //{
        //    parameter = p.Name;
        //    return p;
        //}
    }
}