using System;
using System.Text;

namespace HexMaster.ShortLink.Core.Helpers
{
    public static class ShortCodeGenerator
    {
        public const string ShortCodePool = "abcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly Random Random;


        public static string GenerateShortCode(int maxLength = 6)
        {
            var shortCode = new StringBuilder(maxLength);
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