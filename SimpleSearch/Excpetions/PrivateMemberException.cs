using System;

namespace SimpleSearch.Core.Excpetions
{
    internal class PrivateMemberException : Exception
    {
        internal PrivateMemberException(string message)
            : base(message)
        {
        }

        internal PrivateMemberException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}