using System;
using HexMaster.ShortLink.Core.Exceptions;
using HexMaster.ShortLink.Core.Models.ShortLinks;
using HexMaster.ShortLink.Core.Validators;
using NUnit.Framework;

namespace HexMaster.ShortLink.Core.Tests.Models
{
    [TestFixture]
    public class ShortLinkUpdateTests
    {
        [Test]
        public void WhenModelIsNotAnInstanceOfAnObject_ThenItThrowsArgumentNullException()
        {
            ShortLinkUpdateDto model = null;
            var act = new AsyncTestDelegate(() => ShortLinkUpdateValidator.ValidateModelAsync(model));
            Assert.ThrowsAsync<ArgumentNullException>(act);
        }
 
        [Test]
        public void WhenModelContainsnoShortCode_ThenItThrowsModelValidationException()
        {
            var model = WithUpdateModel();
            model.ShortCode = null;
            var act = new AsyncTestDelegate(() => ShortLinkUpdateValidator.ValidateModelAsync(model));
            Assert.ThrowsAsync<ModelValidationException>(act, "Found a validation error");
        }
        [Test]
        public void WhenModelContainsAnInvalidShortCode_ThenItThrowsModelValidationException()
        {
            var model = WithUpdateModel();
            model.ShortCode = "1234abcd";
            var act = new AsyncTestDelegate(() => ShortLinkUpdateValidator.ValidateModelAsync(model));
            Assert.ThrowsAsync<ModelValidationException>(act, "Found a validation error");
        }
        [Test]
        public void WhenModelContainsNoEndpointUrl_ThenItThrowsModelValidationException()
        {
            var model = WithUpdateModel();
            model.EndpointUrl = null;
            var act = new AsyncTestDelegate(() => ShortLinkUpdateValidator.ValidateModelAsync(model));
            Assert.ThrowsAsync<ModelValidationException>(act, "Found a validation error");
        }
        [Test]
        public void WhenModelContainsAnInvalidEndpointUrl_ThenItThrowsModelValidationException()
        {
            var model = WithUpdateModel();
            model.EndpointUrl = "go-bananas";
            var act = new AsyncTestDelegate(() => ShortLinkUpdateValidator.ValidateModelAsync(model));
            Assert.ThrowsAsync<ModelValidationException>(act, "Found a validation error");
        }

        [Test]
        public void WhenModelIsValid_ThenNoExceptionIsThrown()
        {
            var model = WithUpdateModel();
            var act = new AsyncTestDelegate(() => ShortLinkUpdateValidator.ValidateModelAsync(model));
            Assert.DoesNotThrowAsync(act);
        }


        private static ShortLinkUpdateDto WithUpdateModel()
        {
            return new ShortLinkUpdateDto
            {
                Id = Guid.NewGuid(),
                ShortCode = "abcdefg",
                EndpointUrl = "https://www.target-endpoint.nl",
                ExpirationOn = DateTimeOffset.UtcNow.AddDays(1)
            };
        }

    }
}