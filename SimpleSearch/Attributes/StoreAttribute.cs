using System;
using Lucene.Net.Documents;
using SimpleSearch.Core.Models;

namespace SimpleSearch.Core.Attributes
{
    /// <summary>
    /// Specifys whether a field will be stored in the index so that it can be retrieved later.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StoreAttribute : Attribute
    {
        public StoreAttribute(FieldStore store)
        {
            Store = store;
        }

        internal FieldStore Store { get; set; }

        /// <summary>
        /// Converts to a Lucene Store value
        /// </summary>
        /// <returns></returns>
        internal Field.Store ToStore()
        {
            switch (Store)
            {
                case FieldStore.Yes:
                    return Field.Store.YES;
                case FieldStore.No:
                    return Field.Store.NO;
                default:
                    return Field.Store.YES;
            }
        }
    }
}