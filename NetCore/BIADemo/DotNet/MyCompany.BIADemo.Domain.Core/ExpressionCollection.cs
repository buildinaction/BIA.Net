// <copyright file="ExpressionCollection.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Expression Collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class ExpressionCollection<TEntity> : IEnumerable<KeyValuePair<string, LambdaExpression>>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// The internal dictionary.
        /// </summary>
        private readonly Dictionary<string, LambdaExpression> internalDictionary
            = new Dictionary<string, LambdaExpression>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets or sets the <see cref="LambdaExpression"/> with the specified key.
        /// </summary>
        /// <value>The <see cref="LambdaExpression"/>.</value>
        /// <param name="key">The key.</param>
        /// <returns>The selected <see cref="LambdaExpression"/>.</returns>
        public LambdaExpression this[string key]
        {
            get
            {
                return this.internalDictionary[key];
            }

            set
            {
                this.internalDictionary[key] = value;
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="expression">The expression.</param>
        public void Add<TKey>(string key, Expression<Func<TEntity, TKey>> expression)
        {
            this.internalDictionary.Add(key, expression);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate
        /// through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, LambdaExpression>> GetEnumerator()
        {
            return this.internalDictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The specified key contains key.</returns>
        public bool ContainsKey(string key)
        {
            return this.internalDictionary.ContainsKey(key);
        }
    }
}