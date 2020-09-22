using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Contracts;
using HexMaster.ShortLink.Core.Exceptions;
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

        public Task<List<ShortLinkListItemDto>> ListAsync(string ownerSubjectId, int page, int pageSize)
        {
            return _repository.GetShortLinksListAsync(ownerSubjectId, page, pageSize);
        }

        public async Task<ShortLinkDetailsDto> CreateAsync(string ownerSubjectId, ShortLinkCreateDto dto)
        {
            await ShortLinkCreateValidator.ValidateModelAsync(dto);
            var shortCode = await GenerateUniqueShortLink();
            return await _repository.CreateNewShortLinkAsync(shortCode, dto.EndpointUrl, ownerSubjectId);
        }

        public Task<ShortLinkDetailsDto> ReadAsync(string ownerSubjectId, Guid id)
        {
            return _repository.GetShortLinkDetailsAsync(ownerSubjectId, id);
        }

        public async Task UpdateAsync(string ownerSubjectId, Guid id, ShortLinkUpdateDto dto)
        {
            if (!id.Equals(dto.Id))
            {
                throw new InvalidUpdateRequestException($"Update request for ID '{id}' contains a model with ID '{dto.Id}'");
            }
            await ShortLinkUpdateValidator.ValidateModelAsync(dto);
            if (await _repository.CheckIfShortCodeIsUniqueForShortLinkAsync(id, dto.ShortCode))
            {
                await _repository.UpdateExistingShortLinkAsync(ownerSubjectId,dto);
            }
        }

        public Task DeleteAsync(string ownerSubjectId, Guid id)
        {
            return _repository.DeleteShortLinkAsync(ownerSubjectId, id);
        }

    }
}
