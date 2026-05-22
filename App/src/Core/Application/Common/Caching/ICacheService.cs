using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Caching
{
    public interface ICacheService
    {
        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class;
        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        public Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
