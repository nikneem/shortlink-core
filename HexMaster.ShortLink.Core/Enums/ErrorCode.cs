namespace HexMaster.ShortLink.Core.Enums
{
    public abstract class ErrorCode
    {
        public static readonly ErrorCode InvalidUrl;
        public static readonly ErrorCode[] All;


        public abstract string Code { get; }
        public abstract string Description { get; }
        public abstract string TranslationKey { get; }

        static ErrorCode()
        {
            All = new[]
            {
                InvalidUrl = new InvalidUrlErrorCode()
            };
        }

    }       
    public class InvalidUrlErrorCode : ErrorCode
    {
        public override string Code => "InvalidUrl";
        public override string Description => "The supplied URL is empty or invalid.";
        public override string TranslationKey => $"ErrorCode.{Code}";
    }
}