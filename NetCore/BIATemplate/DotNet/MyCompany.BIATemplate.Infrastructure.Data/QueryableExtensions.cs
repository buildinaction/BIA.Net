// <copyright file="QueryableExtensions.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Infrastructure.Data
{
    using System.Linq;
    using BIA.Net.QueryOrder;
    using MyCompany.BIATemplate.Domain.Core;

    /// <summary>
    /// Extension class for Query interface object.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies the query order.
        /// </summary>
        /// <typeparam name="T">Entity Type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="order">The order.</param>
        /// <returns>Query where order added.</returns>
        public static IQueryable<T> ApplyQueryOrder<T>(this IQueryable<T> source, QueryOrder<T> order)
            where T : class, IEntity
        {
            source = order.GetOrderByList.Aggregate(source, (current, item) => Queryable.OrderBy(current, (dynamic)item));

            source = order.GetOrderByDescendingList.Aggregate(source, (current, item) => Queryable.OrderByDescending(current, (dynamic)item));

            source = order.GetThenByList.Aggregate(source, (current, item) => Queryable.ThenBy((IOrderedQueryable<T>)current, (dynamic)item));

            return order.GetThenByDescendingList.Aggregate(source, (current, item) => Queryable.ThenByDescending((IOrderedQueryable<T>)current, (dynamic)item));
        }
    }
}