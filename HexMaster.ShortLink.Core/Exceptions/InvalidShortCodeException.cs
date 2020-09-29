using HexMaster.ShortLink.Core.Enums;

namespace HexMaster.ShortLink.Core.Exceptions
{
    public class InvalidShortCodeException : ShortLinkException
    {

        public InvalidShortCodeException(string shortCode) :
            base(ErrorCode.InvalidShortCode, $"ShortCode '{}' is not a valid short code")
        {
        }

    }
}
