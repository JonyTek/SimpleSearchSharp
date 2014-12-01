using System;
using Lucene.Net.Search;

namespace SimpleSearch.Core.Helpers
{
    public static class Validate
    {
        /// <summary>
        /// Throws if item is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="message"></param>
        internal static void NotNull<T>(T item, string message)
            where T : class
        {
            if (item == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        /// <summary>
        /// Throws if item is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        internal static void NotNull<T>(T item, string format, params object[] args)
            where T : class
        {
            if (item == null)
            {
                throw new ArgumentNullException(string.Format(format, args));
            }
        }

        /// <summary>
        /// Valides value is within a range
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <param name="value"></param>
        internal static void Range(double lower, double upper, double value, string message)
        {
            if (value < lower || value > upper)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Vallidates a string to not be empty
        /// </summary>
        /// <param name="value"></param>
        internal static void StringNotEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(message);
            }
        }
    }
}