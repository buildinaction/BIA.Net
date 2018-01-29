#region Copyright (c) 2009 S. van Deursen
/* The CuttingEdge.EntitySorting library.
 * 
 * To contact me, please visit my blog at http://www.cuttingedge.it/blogs/steven/ 
 *
 * Copyright (c) 2009 S. van Deursen
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

// NOTE: I placed all related classes in this single file, for easy access and downloading.
namespace BIA.Net.Model.DAL.Sorting
{
    /// <summary>
    /// Specifies a method that filters a collection by returning a filtered collection.
    /// </summary>
    /// <typeparam name="TEntity">The element type of the collection to filter.</typeparam>
    public interface IEntityFilter<TEntity>
    {
        /// <summary>Filters the specified collection.</summary>
        /// <param name="collection">The collection.</param>
        /// <returns>A filtered collection.</returns>
        IQueryable<TEntity> Filter(IQueryable<TEntity> collection);
    }

    /// <summary>Enables filtering of entities.</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public static class EntityFilter<TEntity>
    {
        /// <summary>
        /// Returns a <see cref="IEntityFilter{TEntity}"/> instance that allows construction of
        /// <see cref="IEntityFilter{TEntity}"/> objects though the use of LINQ syntax.
        /// </summary>
        /// <returns>A <see cref="IEntityFilter{TEntity}"/> instance.</returns>
        public static IEntityFilter<TEntity> AsQueryable()
        {
            return new EmptyEntityFilter();
        }

        /// <summary>
        /// Returns a <see cref="IEntityFilter{TEntity}"/> that filters a sequence based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A new <see cref="IEntityFilter{TEntity}"/>.</returns>
        public static IEntityFilter<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return new WhereEntityFilter<TEntity>(predicate);
        }

        /// <summary>An empty entity filter.</summary>
        [DebuggerDisplay("EntityFilter ( Unfiltered )")]
        private sealed class EmptyEntityFilter : IEntityFilter<TEntity>
        {
            /// <summary>Filters the specified collection.</summary>
            /// <param name="collection">The collection.</param>
            /// <returns>A filtered collection.</returns>
            public IQueryable<TEntity> Filter(IQueryable<TEntity> collection)
            {
                // We don't filter, but simply return the collection.
                return collection;
            }

            /// <summary>Returns an empty string.</summary>
            /// <returns>An empty string.</returns>
            public override string ToString()
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Extension methods for the <see cref="IEntityFilter{TEntity}"/> interface.
    /// </summary>
    public static class EntityFilterExtensions
    {
        /// <summary>
        /// Returns a <see cref="IEntityFilter{TEntity}"/> that filters a sequence based on a predicate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="baseFilter">The base filter.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A new <see cref="IEntityFilter{TEntity}"/>.</returns>
        public static IEntityFilter<TEntity> Where<TEntity>(this IEntityFilter<TEntity> baseFilter,
            Expression<Func<TEntity, bool>> predicate)
        {
            if (baseFilter == null)
            {
                throw new ArgumentNullException("baseFilter");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return new WhereEntityFilter<TEntity>(baseFilter, predicate);
        }
    }

    /// <summary>
    /// Filters the collection using a predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [DebuggerDisplay("EntityFilter ( where {ToString()} )")]
    internal sealed class WhereEntityFilter<TEntity> : IEntityFilter<TEntity>
    {
        private readonly IEntityFilter<TEntity> baseFilter;
        private readonly Expression<Func<TEntity, bool>> predicate;

        /// <summary>Initializes a new instance of the <see cref="WhereEntityFilter{TEntity}"/> class.</summary>
        /// <param name="predicate">The predicate.</param>
        public WhereEntityFilter(Expression<Func<TEntity, bool>> predicate)
        {
            this.predicate = predicate;
        }

        /// <summary>Initializes a new instance of the <see cref="WhereEntityFilter{TEntity}"/> class.</summary>
        /// <param name="baseFilter">The base filter.</param>
        /// <param name="predicate">The predicate.</param>
        public WhereEntityFilter(IEntityFilter<TEntity> baseFilter, Expression<Func<TEntity, bool>> predicate)
        {
            this.baseFilter = baseFilter;
            this.predicate = predicate;
        }

        /// <summary>Filters the specified collection.</summary>
        /// <param name="collection">The collection.</param>
        /// <returns>A filtered collection.</returns>
        public IQueryable<TEntity> Filter(IQueryable<TEntity> collection)
        {
            if (this.baseFilter == null)
            {
                return collection.Where(this.predicate);
            }
            else
            {
                return this.baseFilter.Filter(collection).Where(this.predicate);
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string baseFilterPresentation =
                this.baseFilter != null ? this.baseFilter.ToString() : string.Empty;

            // The returned string is used in de DebuggerDisplay.
            if (!string.IsNullOrEmpty(baseFilterPresentation))
            {
                return baseFilterPresentation + ", " + this.predicate.ToString();
            }
            else
            {
                return this.predicate.ToString();
            }
        }
    }
}