// <copyright file="QueryOrderExtensions.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.QueryOrder
{
    using System.Linq.Expressions;

    /// <summary>
    /// Query Order Extensions.
    /// </summary>
    public static class QueryOrderExtensions
    {
        /// <summary>
        /// Gets the by expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="queryOrder">The query order.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="ascending">If set to <c>true</c> [ascending].</param>
        public static void GetByExpression<TEntity>(this QueryOrder<TEntity> queryOrder, LambdaExpression expression, bool ascending)
            where TEntity : class
        {
            if (queryOrder.GetOrderByDescendingList.Count > 0 && queryOrder.GetOrderByList.Count > 0)
            {
                if (ascending)
                {
                    queryOrder.ThenBy(expression);
                }
                else
                {
                    queryOrder.ThenByDescending(expression);
                }
            }
            else
            {
                if (ascending)
                {
                    queryOrder.OrderBy(expression);
                }
                else
                {
                    queryOrder.OrderByDescending(expression);
                }
            }
        }
    }
}