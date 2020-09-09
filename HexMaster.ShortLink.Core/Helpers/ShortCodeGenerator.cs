using System;
using System.Text;

namespace HexMaster.ShortLink.Core.Helpers
{
    public static class ShortCodeGenerator
    {
        private const string LettersOnlyPool = "abcdefghijklmnopqrstuvwxyz";
        private const string ShortCodePool = "abcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly Random Random;


        public static string GenerateShortCode(int maxLength = 6)
        {
            if (maxLength < 2 || maxLength > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "A short code length must be between 2 and 20");
            }
            var shortCode = new StringBuilder(maxLength);
            shortCode.Append(LettersOnlyPool.Substring(Random.Next(0, LettersOnlyPool.Length), 1));
            do
            {
                shortCode.Append(ShortCodePool.Substring(Random.Next(0, ShortCodePool.Length), 1));
            } while (shortCode.Length < maxLength);

            return shortCode.ToString();
        }


        static ShortCodeGenerator()
        {
            Random = new Random(DateTime.Now.Hour*DateTime.Now.Minute*DateTime.Now.Second);
        }

    }
}