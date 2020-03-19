namespace BIA.Net.Business.Helpers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class OrderExtension
    {
        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>
            (this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            bool descending)
        {
            return descending ? source.OrderByDescending(keySelector)
                              : source.OrderBy(keySelector);
        }
    }
}
