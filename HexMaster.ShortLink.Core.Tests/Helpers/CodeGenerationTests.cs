using System;
using System.Text.RegularExpressions;
using HexMaster.ShortLink.Core.Helpers;
using NUnit.Framework;

namespace HexMaster.ShortLink.Core.Tests.Helpers
{
    [TestFixture]
    public class CodeGenerationTests
    {

        private ShortCodeGenerator _generator;

        [SetUp]
        public void Setup()
        {
            _generator = new ShortCodeGenerator();
        }


        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        [TestCase(17)]
        [TestCase(18)]
        [TestCase(19)]
        [TestCase(20)]
        public void WhenShortCodeIsGenerated_ThenAValidShortCodeIsReturned(int length)
        {
            var shortCode = _generator.GenerateShortCode(length);
            Assert.IsTrue(Regex.IsMatch(shortCode, Constants.ShortCodeRegularExpression));
        }

        [TestCase(1)]
        [TestCase(21)]
        public void WhenShortCodeLenthIsInvalid_ThenArgumentOutOfRangeExceptionIsThrown(int length)
        {
            var act = new TestDelegate(() => _generator.GenerateShortCode(length));
            Assert.Throws<ArgumentOutOfRangeException>(act, "Found a validation error");
        }


    }
}
