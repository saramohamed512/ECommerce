using ECommerce.Domain.Contracts;
using ECommerce.ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICachRepository _cacheRepository;
        public CacheService(ICachRepository cacheRepository) 
        {
            _cacheRepository= cacheRepository;

        }

        public async Task<string?> GetAsyn(string CacheKey)
        {
            return await _cacheRepository.GetAsync(CacheKey);
        }

        public async Task SetAsync(string CacheKey, object CacheValue, TimeSpan TimeToLive)
        {
            var Value = JsonSerializer.Serialize(CacheValue);
            await _cacheRepository.SetAsync(CacheKey, Value, TimeToLive);
        }
    }
}
