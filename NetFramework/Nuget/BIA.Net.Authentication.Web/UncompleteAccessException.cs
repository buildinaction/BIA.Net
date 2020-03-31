using System;

namespace BIA.Net.Authentication.Web
{
    public class UncompleteAccessException : UnauthorizedAccessException
    {
        public enum UncompleteType
        {
            MissingFineUserName,
        }
        UncompleteType TypeOfUncompleteAccess { get; }
        public UncompleteAccessException(UncompleteType uType, string message) : base(message)
        {
            TypeOfUncompleteAccess = uType;
        }
    }
}
