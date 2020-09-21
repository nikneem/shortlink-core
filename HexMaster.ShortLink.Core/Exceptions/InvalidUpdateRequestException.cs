using System;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public sealed class InvalidUpdateRequestException : Exception
    {
        public InvalidUpdateRequestException(string message, Exception ex = null)
            : base(message, ex)
        {
        }
    }
}
