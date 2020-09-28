using System;
using HexMaster.ShortLink.Core.Enums;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public sealed class InvalidUpdateRequestException : ShortLinkException
    {
        public InvalidUpdateRequestException(string message, Exception ex = null)
            : base(ErrorCode.InvalidUpdateRequestErrorCode, message, ex)
        {
        }
    }
}
