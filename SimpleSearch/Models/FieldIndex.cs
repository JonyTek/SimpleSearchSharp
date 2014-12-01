namespace SimpleSearch.Core.Models
{
    /// <summary>
    /// Types of analysis performed on a field.
    /// For general use it is recommended that Analyzed be used.
    /// </summary>
    public enum FieldIndex
    {
        /// <summary>
        /// Do not store this within the index.
        /// This means the value cannot be searched but can be retrieved from the index.
        /// </summary>
        No,

        /// <summary>
        /// Index the tokens produced by the analyzer.
        /// Common for text fields
        /// </summary>
        Analyzed,

        /// <summary>
        /// Index the fields value without using an analyzer.
        /// Commonly used for small strings such as product codes
        /// </summary>
        NotAnalyzed,

        /// <summary>
        /// Does not analyze value.
        /// Disable index-time field and document boosting.
        /// </summary>
        NotAnalyzedNoNorms,

        /// <summary>
        /// Index the tokens produced by the analyzer.
        /// Also disable the storing of norms.
        /// See NotAnalyzedNoNorms for details. 
        /// </summary>
        AnalyzedNoNorms
    }
}