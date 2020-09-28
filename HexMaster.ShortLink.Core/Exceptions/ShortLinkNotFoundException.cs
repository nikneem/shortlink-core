using System;
using HexMaster.ShortLink.Core.Enums;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public class ShortLinkNotFoundException : ShortLinkException
    {
        public ShortLinkNotFoundException(Guid id, Exception ex = null) :
            base(ErrorCode.InvalidShortCode, $"ShortLink with ID '{id}' could not be found", ex)
        {
        }
    }
}
