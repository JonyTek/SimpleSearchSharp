using System;

namespace SimpleSearch.Core.Attributes
{
    /// <summary>
    /// Specify that this value is a unique identifier.
    /// Only 1 of these values can be specified per Schema
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IdentifierAttribute : Attribute
    {
    }
}
