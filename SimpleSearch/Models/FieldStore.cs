namespace SimpleSearch.Core.Models
{
    public enum FieldStore
    {
        /// <summary>
        /// Store the value within the index. 
        /// This is useful for short string values such as product code.
        /// If a value is stored it can then be retrieved.
        /// Due to performance it is bad practice to store long strings
        /// </summary>
        Yes,

        /// <summary>
        /// Do not store the value within the index.
        /// This means that the value cannot be retrieved later.        
        /// This option will increase performance
        /// </summary>
        No
    }
}