using System;

namespace SimpleSearch.Core.Excpetions
{
    public class TypeException : Exception
    {
        internal TypeException(string message)
            : base(message)
        {
        }

        internal TypeException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        } 
    }
}