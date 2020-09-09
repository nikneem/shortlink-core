using System;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Contracts;
using HexMaster.ShortLink.Core.Helpers;
using HexMaster.ShortLink.Core.Models;
using HexMaster.ShortLink.Core.Validators;

namespace HexMaster.ShortLink.Core.Services
{
    public class ShortLinksService : IShortLinksService
    {
        private readonly IShortLinksRepository _repository;
        private readonly ShortCodeGenerator _generator;

        public ShortLinksService(
            IShortLinksRepository repository,
            ShortCodeGenerator generator)
        {
            _repository = repository;
            _generator = generator;
        }

        public async Task<string> GenerateUniqueShortLink()
        {
            bool codeIsUnique;
            string shortCode;
            do
            {
                shortCode = _generator.GenerateShortCode();
                codeIsUnique = await _repository.CheckIfShortCodeIsUniqueAsync(shortCode);
            } while (!codeIsUnique);

            return shortCode;
        }

        public async Task<ShortLinkDetailsDto> CreateAsync(ShortLinkCreateDto dto)
        {
            await ShortLinkCreateValidator.ValidateModelAsync(dto);
            var shortCode = await GenerateUniqueShortLink();
            return await _repository.CreateNewShortLinkAsync(shortCode, dto.EndpointUrl, "banana");
        }

        public async Task UpdateAsync(Guid id, ShortLinkUpdateDto dto)
        {
            await ShortLinkUpdateValidator.ValidateModelAsync(dto);
            if (!Equals(id, dto.Id))
            {
                throw new Exception("Unexpected request, cannot update the short link");
            }
            await _repository.UpdateExistingShortLinkAsync(dto);
          
        }
    }
}
