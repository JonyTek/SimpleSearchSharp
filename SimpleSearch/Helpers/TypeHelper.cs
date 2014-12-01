using System.Collections.Generic;
using System.Reflection;
using System;

namespace SimpleSearch.Core.Helpers
{
    internal static class TypeHelper
    {
        /// <summary>
        /// Ctor
        /// </summary>
        static TypeHelper()
        {
            SystemTypes = new HashSet<string>();

            foreach (var type in Assembly.GetExecutingAssembly()
                .GetType()
                .Module
                .Assembly
                .GetExportedTypes())
            {
                SystemTypes.Add(type.FullName);
            }
        }

        /// <summary>
        /// Returns a string collection of System types
        /// </summary>
        internal static HashSet<string> SystemTypes { get; set; }
 
        /// <summary>
        /// Checks to see if a object lives under the Systemnamespace
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsValidType(Type type)
        {
            Validate.NotNull(type, "type");

            // ReSharper disable once PossibleNullReferenceException
            return type.BaseType.FullName == "System.Enum" || SystemTypes.Contains(type.FullName);
        }
    }
}