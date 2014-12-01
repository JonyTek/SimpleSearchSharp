namespace SimpleSearch.Core.Schema
{
    /// <summary>
    /// Base interface for specifying a data schema
    /// </summary>
    public class BaseSchema
    {
        //ReSharper disable once InconsistentNaming
        /// <summary>
        /// Used to access all documents
        /// </summary>
        internal string IDENTITY
        {
            get { return "IDENTITY"; }
        }
    }
}