using System;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public class ShortLinkNotFoundException : Exception
    {
        public ShortLinkNotFoundException(Guid id, Exception ex = null) :
            base($"ShortLink with ID '{id}' could not be found", ex)
        {
        }
    }
}
