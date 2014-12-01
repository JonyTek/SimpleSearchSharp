using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lucene.Net.Analysis;
using Version = Lucene.Net.Util.Version;

namespace SimpleSearch.Core.Helpers
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Creates a new analyzer based on generic argument
        /// </summary>
        /// <typeparam name="TAnalyzer"></typeparam>
        /// <param name="analyzerType"></param>
        /// <returns></returns>
        internal static TAnalyzer Instantiate<TAnalyzer>(this Type analyzerType)
            where TAnalyzer : Analyzer
        {
            var type = typeof (TAnalyzer);
            var ctor = type.GetConstructor(Type.EmptyTypes)
                       ?? type.GetConstructor(new[] {typeof (Version)});

            Validate.NotNull(ctor, "Unknow type {0}, cannot initialize for adding of item to index.",
                type.FullName);

            // ReSharper disable once PossibleNullReferenceException
            var parameters = ctor.GetParameters();

            if (parameters.Any())
            {
                return (TAnalyzer) ctor.Invoke(new object[]
                {
                    Version.LUCENE_30
                });
            }

            return (TAnalyzer) ctor.Invoke(new object[] {});
        }

        /// <summary>
        /// Returns the field name contained within the expression
        /// </summary>
        /// <typeparam name="TSchema"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal static string FieldName<TSchema, TProperty>(this Expression<Func<TSchema, TProperty>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>
        /// Get all properties
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static PropertyInfo[] Properties(this object obj)
        {
            return obj.GetType().Properties();
        }

        /// <summary>
        /// Get all properties
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static PropertyInfo[] Properties(this Type obj)
        {
            return obj.GetProperties(BindingFlags.Public |
                                     BindingFlags.NonPublic |
                                     BindingFlags.Instance |
                                     BindingFlags.Static);
        }
    }
}