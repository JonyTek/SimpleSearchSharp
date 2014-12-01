using System;
using Lucene.Net.Documents;
using SimpleSearch.Core.Models;

namespace SimpleSearch.Core.Attributes
{
    /// <summary>
    /// Specifys the type of analysis to be performed on a field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AnalyzeAttribute : Attribute
    {
        public AnalyzeAttribute(FieldIndex analysis)
        {
            Analysis = analysis;
        }

        internal FieldIndex Analysis { get; set; }

        /// <summary>
        /// Converts a FindIndex to a Lucene Index enum value
        /// </summary>
        /// <returns></returns>
        internal Field.Index ToIndex()
        {
            switch (Analysis)
            {
                case FieldIndex.No:
                    return Field.Index.NO;
                case FieldIndex.Analyzed:
                    return Field.Index.ANALYZED;
                case FieldIndex.AnalyzedNoNorms:
                    return Field.Index.ANALYZED_NO_NORMS;
                case FieldIndex.NotAnalyzed:
                    return Field.Index.NOT_ANALYZED;
                case FieldIndex.NotAnalyzedNoNorms:
                    return Field.Index.NOT_ANALYZED_NO_NORMS;
                default:
                    return Field.Index.ANALYZED;
            }
        }
    }
}