namespace HexMaster.ShortLink.Core.Enums
{
    public abstract class ErrorCode
    {
        public static readonly ErrorCode InvalidUrl;
        public static readonly ErrorCode InvalidShortCode;
        public static readonly ErrorCode ShortCodeNotFoundErrorCode;
        public static readonly ErrorCode InvalidUpdateRequestErrorCode;
        public static readonly ErrorCode[] All;


        public abstract string Code { get; }
        public abstract string Description { get; }
        public abstract string TranslationKey { get; }

        static ErrorCode()
        {
            All = new[]
            {
                InvalidUrl = new InvalidUrlErrorCode(),
                InvalidShortCode = new InvalidShortCodeErrorCode()
            };
        }

    }       
    public class InvalidUrlErrorCode : ErrorCode
    {
        public override string Code => "InvalidUrl";
        public override string Description => "The supplied URL is empty or invalid.";
        public override string TranslationKey => $"ErrorCode.{Code}";
    }
    public class InvalidShortCodeErrorCode : ErrorCode
    {
        public override string Code => "InvalidShortCode";
        public override string Description => "The short code must start with a letter, contain only alphanumeric characters all lower case and must be between 2 and 20 characters long";
        public override string TranslationKey => $"ErrorCode.{Code}";
    }
    public class ShortCodeNotFoundErrorCode : ErrorCode
    {
        public override string Code => "ShortCodeNotFound";
        public override string Description => "The requested ShortCode could not be found";
        public override string TranslationKey => $"ErrorCode.{Code}";
    }
    public class InvalidUpdateRequestErrorCode : ErrorCode
    {
        public override string Code => "InvalidUpdateRequest";
        public override string Description => "The update request was invalid. Nothing was done.";
        public override string TranslationKey => $"ErrorCode.{Code}";
    }
}