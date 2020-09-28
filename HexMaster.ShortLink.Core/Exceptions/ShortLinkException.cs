using System;
using System.Collections.Generic;
using HexMaster.ShortLink.Core.Enums;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public class ShortLinkException : Exception
    {
        public IReadOnlyList<ErrorCode> Errors { get; }

        public ShortLinkException(ErrorCode errorCode, string message, Exception innerException = null)
            : base(message, innerException)
        {
            Errors = new[] {errorCode};
        }
        public ShortLinkException(List<ErrorCode> errorCode, string message, Exception innerException = null)
            : base(message, innerException)
        {
            Errors = errorCode;
        }
    }
}