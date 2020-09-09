using System;
using HexMaster.ShortLink.Core.Exceptions;
using HexMaster.ShortLink.Core.Models.ShortLinks;
using HexMaster.ShortLink.Core.Validators;
using NUnit.Framework;

namespace HexMaster.ShortLink.Core.Tests.Models
{
    [TestFixture]
    public class ShortLinkCreateTests
    {

        [Test]
        public void WhenModelIsNotAnInstanceOfAnObject_ThenItThrowsArgumentNullException()
        {
            ShortLinkCreateDto model = null;
            var act = new AsyncTestDelegate(() => ShortLinkCreateValidator.ValidateModelAsync(model));
            Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Test]
        public void WhenModelContainsAnInvalidUrl_ThenItThrowsModelValidationException()
        {
            var model = new ShortLinkCreateDto
            {
                EndpointUrl = "not-a-valid-url"
            };
            var act = new AsyncTestDelegate(() => ShortLinkCreateValidator.ValidateModelAsync(model));
            Assert.ThrowsAsync<ModelValidationException>(act, "Found a validation error");
        }

        [TestCase("https://www.my-url.com/bananas")]
        [TestCase("https://domains.google")]
        [TestCase("https://hexmaster.nl/article/list?filter=.net")]
        [TestCase("https://hexmaster.nl/article/list?filter=.net&bananas=curved")]
        [TestCase("http://www.my-url.com/bananas")]
        [TestCase("http://domains.google")]
        [TestCase("http://hexmaster.nl/article/list?filter=.net")]
        [TestCase("http://hexmaster.nl/article/list?filter=.net&bananas=curved")]
        public void WhenModelContainsAValidUrl_ThenNoExceptionIsThrown(string url)
        {
            var model = new ShortLinkCreateDto
            {
                EndpointUrl = url
            };
            var act = new AsyncTestDelegate(() => ShortLinkCreateValidator.ValidateModelAsync(model));
            Assert.DoesNotThrowAsync(act);
        }


    }
}