// <copyright file="SearchUtil.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class SearchUtil
    {
        #region String

        public static Expression ContainsMultipleString(object values, Expression propertyExpression)
        {
            if (values is string)
            {
                string[] stringValues = ((string)values).Split(';');

                for (int j = 0; j < stringValues.Count(); j++)
                {
                    if (stringValues[j].StartsWith("\"") && stringValues[j].EndsWith("\""))
                    {
                        stringValues[j] = stringValues[j].Substring(1, stringValues[j].Length - 2);
                    }
                    else if (!(stringValues[j].Contains("*")))
                    {
                        stringValues[j] = string.Format("%{0}%", stringValues[j]);
                    }
                    else
                    {
                        stringValues[j] = stringValues[j].Replace('*', '%');
                    }
                }

                // MethodInfo contains = values.GetType().GetMethod("Contains");
                MethodInfo patIndex = typeof(SqlFunctions).GetMethod("PatIndex");

                // Expression containExpression = Expression.Call(propertyExpression, patIndex, Expression.Constant(stringValues.ElementAt(0)));
                Expression containExpression = Expression.GreaterThan(Expression.Call(patIndex, Expression.Constant(stringValues.ElementAt(0).Trim()), propertyExpression), Expression.Constant(0, typeof(int?)));

                for (int j = 1; j < stringValues.Count(); j++)
                {
                    // containExpression = Expression.Or(containExpression, Expression.Call(propertyExpression, contains, Expression.Constant(stringValues.ElementAt(j))));
                    containExpression = Expression.Or(containExpression, Expression.GreaterThan(Expression.Call(patIndex, Expression.Constant(stringValues.ElementAt(j).Trim()), propertyExpression), Expression.Constant(0, typeof(int?))));
                }

                return containExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression NotContainsMultipleString(object values, Expression propertyExpression)
        {
            return Expression.Not(ContainsMultipleString(values, propertyExpression));
        }

        #endregion

        #region Date

        public static Expression EqualDate(object values, Expression propertyExpression)
        {
            if (values is DateTime)
            {
                MemberExpression hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                Expression resultExpression = Expression.Equal(hasValueExpression, Expression.Constant(true));

                // propertyExpression = Expression.Property(propertyExpression, "Value");
                var datePart = typeof(DbFunctions).GetMethods().FirstOrDefault(mi => mi.Name == "TruncateTime" && mi.GetParameters().Length == 1 && mi.GetParameters().ElementAt(0).ParameterType == typeof(DateTime?));
                Expression truncateDate = Expression.Call(datePart, propertyExpression);
                resultExpression = Expression.AndAlso(resultExpression, Expression.Equal(truncateDate, Expression.Constant(((DateTime)values).Date, typeof(DateTime?))));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression NotEqualDate(object values, Expression propertyExpression)
        {
            return Expression.Not(EqualDate(values, propertyExpression));
        }

        public static Expression GreaterThanDate(object values, Expression propertyExpression)
        {
            if (values is DateTime)
            {
                MemberExpression hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                Expression resultExpression = Expression.Equal(hasValueExpression, Expression.Constant(true));

                // propertyExpression = Expression.Property(propertyExpression, "Value");
                var datePart = typeof(DbFunctions).GetMethods().FirstOrDefault(mi => mi.Name == "TruncateTime" && mi.GetParameters().Length == 1 && mi.GetParameters().ElementAt(0).ParameterType == typeof(DateTime?));
                Expression truncateDate = Expression.Call(datePart, propertyExpression);
                resultExpression = Expression.AndAlso(resultExpression, Expression.GreaterThan(truncateDate, Expression.Constant(((DateTime)values).Date, typeof(DateTime?))));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression GreaterThanOrEqualDate(object values, Expression propertyExpression)
        {
            if (values is DateTime)
            {
                MemberExpression hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                Expression resultExpression = Expression.Equal(hasValueExpression, Expression.Constant(true));

                // propertyExpression = Expression.Property(propertyExpression, "Value");
                var datePart = typeof(DbFunctions).GetMethods().FirstOrDefault(mi => mi.Name == "TruncateTime" && mi.GetParameters().Length == 1 && mi.GetParameters().ElementAt(0).ParameterType == typeof(DateTime?));
                Expression truncateDate = Expression.Call(datePart, propertyExpression);
                resultExpression = Expression.AndAlso(resultExpression, Expression.GreaterThanOrEqual(truncateDate, Expression.Constant(((DateTime)values).Date, typeof(DateTime?))));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression LessThanDate(object values, Expression propertyExpression)
        {
            if (values is DateTime)
            {
                MemberExpression hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                Expression resultExpression = Expression.Equal(hasValueExpression, Expression.Constant(true));

                // propertyExpression = Expression.Property(propertyExpression, "Value");
                var datePart = typeof(DbFunctions).GetMethods().FirstOrDefault(mi => mi.Name == "TruncateTime" && mi.GetParameters().Length == 1 && mi.GetParameters().ElementAt(0).ParameterType == typeof(DateTime?));
                Expression truncateDate = Expression.Call(datePart, propertyExpression);
                resultExpression = Expression.AndAlso(resultExpression, Expression.LessThan(truncateDate, Expression.Constant(((DateTime)values).Date, typeof(DateTime?))));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression LessThanOrEqualDate(object values, Expression propertyExpression)
        {
            if (values is DateTime)
            {
                MemberExpression hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                Expression resultExpression = Expression.Equal(hasValueExpression, Expression.Constant(true));

                // propertyExpression = Expression.Property(propertyExpression, "Value");
                var datePart = typeof(DbFunctions).GetMethods().FirstOrDefault(mi => mi.Name == "TruncateTime" && mi.GetParameters().Length == 1 && mi.GetParameters().ElementAt(0).ParameterType == typeof(DateTime?));
                Expression truncateDate = Expression.Call(datePart, propertyExpression);
                resultExpression = Expression.AndAlso(resultExpression, Expression.LessThanOrEqual(truncateDate, Expression.Constant(((DateTime)values).Date, typeof(DateTime?))));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        #endregion

        #region Bool

        public static Expression EqualBool(object values, Expression propertyExpression)
        {
            if (values is bool)
            {
                propertyExpression = Expression.Convert(propertyExpression, typeof(bool?));

                return Expression.Equal(propertyExpression, Expression.Constant(values, typeof(bool?)));
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression NotEqualBool(object values, Expression propertyExpression)
        {
            return Expression.Not(EqualBool(values, propertyExpression));
        }

        #endregion

        #region Int

        public static Expression EqualInt(object values, Expression propertyExpression)
        {
            if (values is int || values is short)
            {
                return Expression.Equal(propertyExpression, Expression.Constant(values));
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression NotEqualInt(object values, Expression propertyExpression)
        {
            return Expression.Not(EqualInt(values, propertyExpression));
        }

        public static Expression GreaterThanInt(object values, Expression propertyExpression)
        {
            if (values is int || values is short)
            {
                Expression resultExpression = Expression.GreaterThan(propertyExpression, Expression.Constant(values));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression GreaterThanOrEqualInt(object values, Expression propertyExpression)
        {
            if (values is int || values is short)
            {
                Expression resultExpression = Expression.GreaterThanOrEqual(propertyExpression, Expression.Constant(values));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression LessThanInt(object values, Expression propertyExpression)
        {
            if (values is int || values is short)
            {
                Expression resultExpression = Expression.LessThan(propertyExpression, Expression.Constant(values));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        public static Expression LessThanOrEqualInt(object values, Expression propertyExpression)
        {
            if (values is int || values is short)
            {
                Expression resultExpression = Expression.LessThanOrEqual(propertyExpression, Expression.Constant(values));
                return resultExpression;
            }
            else
            {
                throw new FormatException("The object didn't contains the right type of variable in the search method");
            }
        }

        #endregion

        #region MultiInt

        public static Expression EqualMultiInt(object values, Expression propertyExpression)
        {
            if (values is IEnumerable<int>)
            {
                if (((MemberExpression)propertyExpression).Member.MemberType == MemberTypes.Property)
                {
                    var subType = ((PropertyInfo)((MemberExpression)propertyExpression).Member).PropertyType;
                    if (subType.Namespace == "System.Collections.Generic")
                    {
                        Type entityType = subType.GetGenericArguments().First();

                        ParameterExpression parameterExpression = Expression.Parameter(entityType);
                        MemberExpression idPropertyExpression = Expression.Property(parameterExpression, "Id");
                        var lambdaId = Expression.Lambda(idPropertyExpression, parameterExpression);

                        MethodInfo select = typeof(Enumerable).GetMethods().FirstOrDefault(mi => mi.Name == "Select" && mi.GetParameters().Length == 2).MakeGenericMethod(entityType, typeof(int));
                        MethodInfo intersect = typeof(Enumerable).GetMethods().FirstOrDefault(mi => mi.Name == "Intersect").MakeGenericMethod(typeof(int));
                        MethodInfo count = typeof(Enumerable).GetMethods().FirstOrDefault(mi => mi.Name == "Count" && mi.GetParameters().Length == 1).MakeGenericMethod(typeof(int));
                        Expression collectionExpression = Expression.Call(select, propertyExpression, lambdaId);
                        collectionExpression = Expression.Call(intersect, collectionExpression, Expression.Constant(values, typeof(IEnumerable<int>)));
                        collectionExpression = Expression.Call(count, collectionExpression);
                        collectionExpression = Expression.Equal(collectionExpression, Expression.Constant(((IEnumerable<int>)values).Count()));

                        return collectionExpression;
                    }
                    else
                    {
                        propertyExpression = Expression.Property(propertyExpression, "Id");
                        MethodInfo contains = typeof(Enumerable).GetMethods().FirstOrDefault(mi => mi.Name == "Contains" && mi.GetParameters().Length == 2).MakeGenericMethod(typeof(int));
                        return Expression.Call(contains, Expression.Constant(values, typeof(IEnumerable<int>)), propertyExpression);
                    }
                }
            }

            throw new FormatException("The object didn't contains the right type of variable in the search method");
        }

        public static Expression NotEqualMultiInt(object values, Expression propertyExpression)
        {
            return Expression.Not(EqualMultiInt(values, propertyExpression));
        }

        #endregion
    }
}
