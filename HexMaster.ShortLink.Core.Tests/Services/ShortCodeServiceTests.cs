using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Contracts;
using HexMaster.ShortLink.Core.Helpers;
using HexMaster.ShortLink.Core.Services;
using Moq;
using NUnit.Framework;

namespace HexMaster.ShortLink.Core.Tests.Services
{
    public class ShortCodeServiceTests
    {

        private Mock<IShortLinksRepository> _shortLinksRepositoryMock;
        private ShortLinksService _service;

        [SetUp]
        public void Setup()
        {
            _shortLinksRepositoryMock = new Mock<IShortLinksRepository>();

            var generator = new ShortCodeGenerator();
            _service = new ShortLinksService(
                _shortLinksRepositoryMock.Object,
                generator);
        }

        [Test]
        public async Task WhenNewShortCodeIsGenerated_ThenTheCodeIsValidAndUnique()
        {
            WithShortCodeMarkedAsUnique();
            var shortCode = await _service.GenerateUniqueShortLink();
            Assert.AreEqual(Constants.DefaultShortLinkLength, shortCode.Length);
        }

        [Test]
        public async Task WhenNewShortCodeIsGenerdated_ThenTheCodeIsValidAndUnique()
        {
            WithFirstGeneratedCodeNotUnique();
            var shortCode = await _service.GenerateUniqueShortLink();
            Assert.AreEqual(Constants.DefaultShortLinkLength, shortCode.Length);
            _shortLinksRepositoryMock.Verify(mck => 
                mck.CheckIfShortCodeIsUniqueAsync(It.IsAny<string>()),
                Times.Exactly(2));
        }


        private void WithShortCodeMarkedAsUnique()
        {
            _shortLinksRepositoryMock
                .Setup(r => r.CheckIfShortCodeIsUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
        }
        private void WithFirstGeneratedCodeNotUnique()
        {
            _shortLinksRepositoryMock.SetupSequence(x => x.CheckIfShortCodeIsUniqueAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false))
                .Returns(Task.FromResult(true));
        }

    }
}
