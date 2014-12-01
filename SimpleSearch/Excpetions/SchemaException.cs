using System;

namespace SimpleSearch.Core.Excpetions
{
    public class SchemaException : Exception
    {
        public SchemaException()
            : base("Schemas do not match. Schemas cannot be updated after they are configured at initialization.")
        {
        }
    }
}
