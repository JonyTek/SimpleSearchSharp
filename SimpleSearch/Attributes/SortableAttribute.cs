using System;

namespace SimpleSearch.Core.Attributes
{
    /// <summary>
    /// Used to allow a field to be sortable
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SortableAttribute : Attribute
    {
        internal string DisplayName { get; set; }

        public SortableAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
