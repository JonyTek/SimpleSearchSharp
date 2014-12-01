using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lucene.Net.Search;
using SimpleSearch.Core.Attributes;
using SimpleSearch.Core.Schema;

namespace SimpleSearch.Core.Helpers
{
    /// <summary>
    /// Creates sort objects
    /// </summary>
    /// <typeparam name="TSchema"></typeparam>
    public class SortBuilder<TSchema>
        where TSchema : BaseSchema, new()
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static SortBuilder<TSchema> _instance;

        /// <summary>
        /// Thread lock for below
        /// </summary>
        // ReSharper disable once StaticFieldInGenericType
        private static readonly object ThreadLock = new object();

        /// <summary>
        /// Private ctor
        /// </summary>
        private SortBuilder()
        {
        }

        /// <summary>
        /// Singleton implementation
        /// </summary>
        internal static SortBuilder<TSchema> Instance
        {
            get
            {
                lock (ThreadLock)
                {
                    return _instance
                           ?? (_instance = new SortBuilder<TSchema>());
                }
            }
        }


        /// <summary>
        /// Returns a sort object based on the pass expression
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Sort SortBy<TProperty>(Expression<Func<TSchema, TProperty>> expression)
        {
            return expression == null
                ? Sort.RELEVANCE
                : new Sort(new SortField(expression.FieldName(), CultureInfo.CurrentCulture, true));
        }

        /// <summary>
        /// Returns a sort object based on the pass expression
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Sort SortBy(string fieldName)
        {
            return fieldName == null || fieldName.Equals("RELEVANCE")
                ? Sort.RELEVANCE
                : new Sort(new SortField(fieldName, CultureInfo.CurrentCulture, true));
        }

        /// <summary>
        /// Returns all fileds that are decorated with a Sortable attribute.
        /// Key = name & value = Display name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSortableFields(bool removeRelevance = false)
        {
            var dictionary = new Dictionary<string, string>();
            if (!removeRelevance)
            {
                dictionary.Add("RELEVANCE", "Relevance");
            }
            
            var properties = typeof (TSchema).Properties();
            var sortableFields = properties.Where(prop => prop.GetCustomAttribute<SortableAttribute>() != null);
            foreach (var field in sortableFields)
            {
                var displayName = field.GetCustomAttribute<SortableAttribute>().DisplayName;
                var name = field.Name;
                dictionary.Add(name, displayName);
            }

            return dictionary;
        } 
    }
}