using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Domain.Caching;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Caching.Memory
{
    public class MemoryCacheService : ICacheService
    {
        private readonly Dictionary<string, string> _store = new();

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class
        {
            if(_store.TryGetValue(key, out var value))
            {
                T? convertedVal = JsonSerializer.Deserialize<T>(value!);
                return Task.FromResult<T?>(convertedVal);
            }
            else
            {
                return Task.FromResult<T?>(null);
            }
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            _store.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveItemsByPatternAsync(string pattern)
        {
            var keysToRemove = _store.Keys.Where(k => IsMatch(k, pattern)).ToList();
            foreach (var key in keysToRemove)
            {
                _store.Remove(key);
            }
            return Task.CompletedTask;
        }

        private static bool IsMatch(string text, string pattern)
        {
            string regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$";
            return Regex.IsMatch(text, regexPattern);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            _store[key] = JsonSerializer.Serialize(value!);
            return Task.CompletedTask;
        }
    }
}
