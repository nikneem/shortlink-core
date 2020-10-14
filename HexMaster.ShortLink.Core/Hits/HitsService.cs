using System;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Contracts;
using HexMaster.ShortLink.Core.Hits.Contracts;

namespace HexMaster.ShortLink.Core.Hits
{
    public sealed class HitsService : IHitsService
    {
        private readonly IHitsRepository _repository;
        private readonly IShortLinksRepository _shortlinksRepository;

        public async Task RegisterHitAsync(string shortCode, DateTimeOffset eventDate)
        {
            await _repository.CreateNewAsync(eventDate, shortCode);
            await _shortlinksRepository.IncreaseHitsAsync(shortCode);
        }

        public async Task RestoreHitsCount()
        {
            var totalHits = await _repository.GetHitsPerShortCode();
            foreach (var hit in totalHits)
            {
                await _shortlinksRepository.SetHitsAsync(hit.Item1, hit.Item2);
            }
        }

        public HitsService(IHitsRepository repository,
            IShortLinksRepository shortlinksRepository)
        {
            _repository = repository;
            _shortlinksRepository = shortlinksRepository;
        }


    }
}