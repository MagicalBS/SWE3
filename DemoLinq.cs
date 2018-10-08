using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Linq.Expressions;

namespace Linq
{
    public class DemoLinq<T> : IQueryable<T>
    {
        private Expression _expression = null;
        private DemoLinqProvider _provider = null;
        public DemoLinq()
        {
            _expression = Expression.Constant(this);
            _provider = new DemoLinqProvider();
        }

        internal DemoLinq(DemoLinqProvider provider, Expression expression)
        {
            _expression = expression;
            _provider = provider;
        }

        public Type ElementType => typeof(T);

        public Expression Expression => _expression;

        public IQueryProvider Provider => _provider;

        public IEnumerator<T> GetEnumerator()
        {
            // Returns a enumeration (ToList, ToArray, foreach, ...)
            return _provider.GetEnumerator<T>(_expression);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // Returns a enumeration (ToList, ToArray, foreach, ...)
            return _provider.GetEnumerator<T>(_expression);
        }

        public string GetSqlStatement()
        {
            return _provider.GetSqlStatement();
        }
    }

    internal class DemoLinqProvider : IQueryProvider
    {
        private string sqlStatement = "";

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new DemoLinq<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            // returns a single object (First, Single, etc)
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            // returns a single object (First, Single, etc)
            throw new NotImplementedException();
        }

        internal IEnumerator<T> GetEnumerator<T>(Expression expression)
        {
            // Returns an enumeration (ToList, ToArray, foreach, ...)
            var visitor = new SqlVisitor();
            visitor.Visit(expression);
            sqlStatement = visitor.GetSqlStatement();

            return new MyTable[]
            {
                new MyTable() { FirstName = "Peter", Age = 32 },
                new MyTable() { FirstName = "Marie", Age = 38 },
            }
            .OfType<T>()
            .GetEnumerator();
        }

        public string GetSqlStatement()
        {
            return sqlStatement;
        }
    }
}
