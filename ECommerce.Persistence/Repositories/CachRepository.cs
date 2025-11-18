using ECommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    

    public class CachRepository : ICachRepository
    {
        private readonly IDatabase _database;
        public CachRepository(IConnectionMultiplexer connection) 
        { 
            _database= connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string CacheKey)
        {
            var cacheValue= await  _database.StringGetAsync(CacheKey);
            if (cacheValue.IsNullOrEmpty)
                return null;
            return cacheValue.ToString();
        }

        public async Task SetAsync(string CacheKey, string CasheValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(CacheKey, CasheValue, TimeToLive);
        }
    }
}
