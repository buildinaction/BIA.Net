using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BIA.Net.Common.Helpers
{
    /// <summary>
    /// Helper class for type.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Gets the detailed name of a property or method.
        /// </summary>
        /// <remarks>If c#6 is available, use the nameof operator instead.</remarks>
        /// <typeparam name="T">Type of the class.</typeparam>
        /// <param name="expression">Member expression to select the property or method.</param>
        /// <returns>The string equivalent of the expression.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Generic type usage with a static class is the target of this method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nested types are intented for lambda expression usage.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Base class is not used to avoid specifying type in usage.")]
        public static string NameOf<T>(Expression<Func<T>> expression)
        {
            return NameOf(expression, true);
        }

        /// <summary>
        /// Gets the name of a property or method.
        /// </summary>
        /// <remarks>If c#6 is available, use the nameof operator instead.</remarks>
        /// <typeparam name="T">Type of the class.</typeparam>
        /// <param name="expression">Member expression to select the property or method.</param>
        /// <param name="detailed">If true, the whole expression hierarchy to acces the field will be available.</param>
        /// <returns>The string equivalent of the expression.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Generic type usage with a static class is the target of this method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nested types are intented for lambda expression usage.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Base class is not used to avoid specifying type in usage.")]
        public static string NameOf<T>(Expression<Func<T>> expression, bool detailed)
        {
            #region Check parameter

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            #endregion Check parameter

            List<string> outputData = ExtractNamePart(expression.Body, detailed);
            return outputData != null ? string.Join(".", outputData) : null;
        }

        /// <summary>
        /// Extract the different part of the provided expression.
        /// </summary>
        /// <param name="expression">Expression body to handle.</param>
        /// <param name="detailed">If true, the whole expression hierarchy to acces the field will be available.</param>
        /// <returns>The different part of the provided expression.</returns>
        internal static List<string> ExtractNamePart(Expression expression, bool detailed)
        {
            #region Check parameters

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            #endregion Check parameters

            List<string> outputData = new List<string>();

            MemberExpression memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                // Expression targets a member
                outputData.Add(memberExpression.Member.Name);
                if (detailed)
                {
                    // Recursively extract name from parent expression
                    MemberExpression previous = memberExpression.Expression as MemberExpression;
                    while (previous != null)
                    {
                        outputData.Add(previous.Member.Name);
                        previous = previous.Expression as MemberExpression;
                    }
                }
            }
            else
            {
                MethodCallExpression methodCallExpression = expression as MethodCallExpression;
                if (methodCallExpression != null)
                {
                    // Expression targets a method
                    outputData.Add(methodCallExpression.Method.Name);
                    if (detailed)
                    {
                        // Recursively extract name from parent expression
                        MemberExpression previous = methodCallExpression.Object as MemberExpression;
                        while (previous != null)
                        {
                            outputData.Add(previous.Member.Name);
                            previous = previous.Expression as MemberExpression;
                        }
                    }
                }
            }

            if (!outputData.Any())
            {
                throw new ArgumentException("Provided expression should be a member or method call expression", "expression");
            }
            else
            {
                // Reverse order as name were added recursively but started from top
                outputData.Reverse();
            }

            return outputData;
        }
        public static Type GetTypeFromString(string sType)
        {
            Type type = null;
            foreach (var _a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var _t in _a.GetTypes())
                {
                    try
                    {
                        if ((_t.FullName == sType) /*&& _t.IsClass*/)
                        {
                            type = _t;
                            break;
                        }
                    }
                    catch { }
                }
                if (type != null) break;
            }
            return type;
        }
    }

    /// <summary>
    /// Helper class for type.
    /// </summary>
    /// <typeparam name="TClass">Type managed by the helper class.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Gather same kind of type class.")]
    public static class TypeHelper<TClass>
    {
        /// <summary>
        /// Gets the detailed name of a property or method.
        /// </summary>
        /// <remarks>If c#6 is available, use the nameof operator instead.</remarks>
        /// <typeparam name="T">Type of the class.</typeparam>
        /// <param name="expression">Member expression to select the property or method.</param>
        /// <returns>The string equivalent of the expression.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Generic type usage with a static class is the target of this method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nested types are intented for lambda expression usage.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Base class is not used to avoid specifying type in usage.")]
        public static string NameOf<T>(Expression<Func<TClass, T>> expression)
        {
            return NameOf(expression, true);
        }

        /// <summary>
        /// Gets the name of a property or method.
        /// </summary>
        /// <remarks>If c#6 is available, use the nameof operator instead.</remarks>
        /// <typeparam name="T">Type of the class.</typeparam>
        /// <param name="expression">Member expression to select the property or method.</param>
        /// <param name="detailed">If true, the whole expression hierarchy to acces the field will be available.</param>
        /// <returns>The string equivalent of the expression.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Generic type usage with a static class is the target of this method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nested types are intented for lambda expression usage.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Base class is not used to avoid specifying type in usage.")]
        public static string NameOf<T>(Expression<Func<TClass, T>> expression, bool detailed)
        {
            #region Check parameter

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            #endregion Check parameter

            List<string> outputData = TypeHelper.ExtractNamePart(expression.Body, detailed);
            return outputData != null ? string.Join(".", outputData) : null;
        }
    }
}
