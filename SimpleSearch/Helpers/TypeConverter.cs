using System.Collections;
using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using SimpleSearch.Core.Attributes;
using SimpleSearch.Core.Excpetions;
using SimpleSearch.Core.Models;
using SimpleSearch.Core.Schema;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SimpleSearch.Core.Helpers
{
    /// <summary>
    /// Performs type conversions between Lucene document to type T and vice-versa
    /// </summary>
    internal static class TypeConverter
    {
        /// <summary>
        /// Converts an object to a Lucene document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        internal static Document ToDocument<T>(this T item)
            where T : BaseSchema, new()
        {
            var properties = item.Properties();
            Validate.NotNull(properties, "Type: {0} must contain properties.", item.GetType().Name);

            var idField = properties.FirstOrDefault(f => f.GetCustomAttribute<IdentifierAttribute>() != null);
            Validate.NotNull(idField, "You must provide an identifier property. Please see the [IdentifierAttribute]");

            var document = new Document();

            foreach (var property in properties)
            {
                var prop = property;
                if (!TypeHelper.IsValidType(prop.PropertyType))
                    throw new TypeException(string.Format(
                        "Type: {0} is not allowed in this context. Please stick to Types defined within the System namespace.",
                        prop.MemberType));

                var index = property.GetCustomAttribute<AnalyzeAttribute>()
                            ?? new AnalyzeAttribute(FieldIndex.Analyzed);
                var store = property.GetCustomAttribute<StoreAttribute>()
                            ?? new StoreAttribute(FieldStore.Yes);

                var value = prop.PropertyType == typeof (DateTime)
                    ? ((DateTime) property.GetValue(item)).ToString(Constants.QueryDateFormat)
                    : property.GetValue(item)
                      ?? string.Empty;
                
                var name = property.Name;

                //Validate.NotNull(value, "You must set the value of field '{0}'", name);

                document.Add(new Field(name, value.ToString(), store.ToStore(), index.ToIndex()));
            }

            return document;
        }

        /// <summary>
        /// Converta a Lucene document back to a type of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <returns></returns>
        internal static T ToSchema<T>(this Document document)
            where T : BaseSchema, new()
        {
            var item = new T();
            var itemProperties = item.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttribute<StoreAttribute>() != null &&
                            p.GetCustomAttribute<StoreAttribute>().Store == FieldStore.Yes);

            var documentFields = document.GetFields();

            foreach (var property in itemProperties)
            {
                var prop = property;

                if (!TypeHelper.IsValidType(prop.PropertyType))
                    throw new TypeException(string.Format(
                        "Type: {0} is not allowed. Schema has changed since inserting data.",
                        prop.MemberType));

                if (!property.CanWrite)
                    throw new PrivateMemberException("Must provide public setter for '{0}'", property.Name);

                var field = documentFields.FirstOrDefault(f => f.Name.Equals(property.Name));

                try
                {
                    property.SetValue(item, prop.ToType(field), null);
                }
                catch (NotSupportedException)
                {
                    throw new TypeException(
                        "Type: {0} not supported. Please stick to basic types. Complex types not supported.",
                        prop.PropertyType.FullName);
                }
            }

            return item;
        }

        /// <summary>
        /// Converts a string to object of type T
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static object ToType(this PropertyInfo prop, IFieldable field)
        {
            if (prop.PropertyType.FullName == "System.DateTime")
            {
                return DateTime.ParseExact(field.StringValue, Constants.QueryDateFormat, null);
            }

            var converter = TypeDescriptor.GetConverter(prop.PropertyType);
            
            // ReSharper disable once AssignNullToNotNullAttribute
            return converter.ConvertFromString(null, CultureInfo.InvariantCulture, field.StringValue);
        }
    }
}
