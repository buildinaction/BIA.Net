// <copyright file="MapperBase.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Business.DTO.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Extension to translate Entity in DTO
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Merges 2 expressions.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <typeparam name="TBaseEntity">The type of the base entity.</typeparam>
        /// <typeparam name="TBaseDto">The type of the base dto.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="baseExpression">The base expression.</param>
        /// <returns>Expresion corresponding to the 2 expressions merged</returns>
        public static Expression<Func<TEntity, TDto>> MergeWith<TEntity, TDto, TBaseEntity, TBaseDto>(
            this Expression<Func<TEntity, TDto>> expression,
            Expression<Func<TBaseEntity, TBaseDto>> baseExpression)
        {
            var body = (MemberInitExpression)expression.Body;
            var param = expression.Parameters[0];
            List<MemberBinding> bindings = new List<MemberBinding>(body.Bindings.OfType<MemberAssignment>());

            var baseExpressionBody = (MemberInitExpression)baseExpression.Body;
            var replace = new ParameterReplaceVisitor(baseExpression.Parameters[0], param);
            foreach (var binding in baseExpressionBody.Bindings.OfType<MemberAssignment>())
            {
                bindings.Add(Expression.Bind(
                    binding.Member,
                    replace.VisitAndConvert(binding.Expression, "MergeWith")));
            }

            return Expression.Lambda<Func<TEntity, TDto>>(
                Expression.MemberInit(body.NewExpression, bindings), param);
        }

        /// <summary>
        /// Convert Entity to DTO.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>DTO full with entity fileds values</returns>
        public static TDto ToDTO<TEntity, TDto>(
            this TEntity entity,
            Expression<Func<TEntity, TDto>> expression)
        {
            return new List<TEntity>() { entity }.AsQueryable().Select(expression).SingleOrDefault();
        }

        /// <summary>
        /// Convert List of Entity to List of DTO.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>List of DTO.</returns>
        public static IQueryable<TDto> ToListDTO<TEntity, TDto>(
            this IQueryable<TEntity> entities,
            Expression<Func<TEntity, TDto>> expression)
        {
            return entities.Select(expression);
        }

        /// <summary>
        /// Helper class for merge
        /// </summary>
        /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
        private class ParameterReplaceVisitor : ExpressionVisitor
        {
            /// <summary>
            /// From
            /// </summary>
            private readonly ParameterExpression from;

            /// <summary>
            /// To
            /// </summary>
            private readonly ParameterExpression to;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterReplaceVisitor"/> class.
            /// </summary>
            /// <param name="from">From.</param>
            /// <param name="to">To.</param>
            public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
            {
                this.from = from;
                this.to = to;
            }

            /// <summary>
            /// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression" />.
            /// </summary>
            /// <param name="node">The expression to visit.</param>
            /// <returns>
            /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
            /// </returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == from ? to : base.VisitParameter(node);
            }
        }
    }
}