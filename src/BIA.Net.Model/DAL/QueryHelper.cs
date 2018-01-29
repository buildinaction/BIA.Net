using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Model.DAL
{
    public static class QueryHelper
    {
        public static IOrderedQueryable<TItem> OrderBy<TItem>(IQueryable<TItem> source, string propertyName, bool descending)
        {
            // return OrderingHelper(source, propertyName, descending, false);
            if (!descending)
            {
                return Sorting.EntitySorter<TItem>.OrderBy(propertyName).Sort(source);
            }
            else
            {
                return Sorting.EntitySorter<TItem>.OrderByDescending(propertyName).Sort(source);
            }
        }

        public static IOrderedQueryable<TItem> ThenBy<TItem>(IOrderedQueryable<TItem> source, string propertyName, bool descending)
        {
            return OrderingHelper(source, propertyName, descending, true);
        }

        public static IQueryable<T> ColumnSearch<T>(IQueryable<T> query, string sSearch, List<string> columns) /*columns.Where(c => c.Searchable == true) .. . SName*/
        {
            Expression searchExpression = null;
            Expression sSearchValueExpression = Expression.Constant(sSearch);
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var parameterExp = Expression.Parameter(typeof(T), "type");

            foreach (string column in columns)
            {
                Expression propertyExpression = null;

                foreach (string propertyName in column.Split('.'))
                {
                    if (propertyExpression == null)
                    {
                        propertyExpression = Expression.Property(parameterExp, propertyName);
                    }
                    else
                    {
                        propertyExpression = Expression.Property(propertyExpression, propertyName);
                    }
                }

                if (propertyExpression.Type == typeof(DateTime) || propertyExpression.Type == typeof(DateTime?))
                {
                    propertyExpression = DateTimeExpressionToString(propertyExpression);

                }
                else
                {
                    MethodInfo toStringMethod = typeof(object).GetMethod("ToString");
                    propertyExpression = Expression.Call(propertyExpression, toStringMethod);
                }

                var containsExp = Expression.Call(propertyExpression, containsMethod, sSearchValueExpression);
                if (searchExpression != null)
                {
                    searchExpression = Expression.OrElse(searchExpression, containsExp);
                }
                else
                {
                    searchExpression = containsExp;
                }
            }

            if (searchExpression != null)
            {
                query = query.Where(Expression.Lambda<Func<T, bool>>(searchExpression, parameterExp));
            }

            return query;
        }

        private static Expression DateTimeExpressionToString(Expression propertyExpression)
        {
            MethodInfo toStringMethod = typeof(object).GetMethod("ToString");
            MethodInfo sqlFunctionDatePartMethod = typeof(SqlFunctions).GetMethod("DatePart", new[] { typeof(string), typeof(Nullable<DateTime>) });
            MethodInfo stringConcat = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });

            Expression dayExpression = AddZeroForDateFormat(Expression.Call(Expression.Call(sqlFunctionDatePartMethod, Expression.Constant("day"), Expression.Convert(propertyExpression, typeof(DateTime?))), toStringMethod));
            Expression monthExpression = AddZeroForDateFormat(Expression.Call(Expression.Call(sqlFunctionDatePartMethod, Expression.Constant("month"), Expression.Convert(propertyExpression, typeof(DateTime?))), toStringMethod));
            Expression yearExpression = Expression.Call(Expression.Call(sqlFunctionDatePartMethod, Expression.Constant("year"), Expression.Convert(propertyExpression, typeof(DateTime?))), toStringMethod);

            Expression tmpDateExpression = dayExpression;
            tmpDateExpression = Expression.Add(tmpDateExpression, Expression.Constant("/"), stringConcat);
            tmpDateExpression = Expression.Add(tmpDateExpression, monthExpression, stringConcat);
            tmpDateExpression = Expression.Add(tmpDateExpression, Expression.Constant("/"), stringConcat);
            tmpDateExpression = Expression.Add(tmpDateExpression, yearExpression, stringConcat);

            return tmpDateExpression;
        }

        private static IOrderedQueryable<TItem> OrderingHelper<TItem>(IQueryable<TItem> source, string propertyName, bool descending, bool anotherLevel)
        {
            var param = Expression.Parameter(typeof(TItem), "p");
            var property = Expression.PropertyOrField(param, propertyName);
            var sort = Expression.Lambda(property, param);

            var call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(TItem), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<TItem>)source.Provider.CreateQuery<TItem>(call);
        }

        private static Expression AddZeroForDateFormat(Expression expression)
        {
            MethodInfo stringConcat = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });
            return Expression.Condition(Expression.Equal(Expression.Property(expression, "Length"), Expression.Constant(2)),
                expression,
                Expression.Add(Expression.Constant("0"), expression, stringConcat));
        }
    }
}
