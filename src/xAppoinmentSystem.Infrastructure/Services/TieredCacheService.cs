using System.Collections;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using xAppoinmentSystem.Application.Interfaces;

namespace xAppoinmentSystem.Infrastructure.Services;

public class TieredCacheService : ITieredCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly TimeSpan _defaultExpiry = TimeSpan.FromMinutes(30);
    private readonly TimeSpan _memoryExpiry = TimeSpan.FromMinutes(5);

    public TieredCacheService(IMemoryCache memoryCache, IDistributedCache distributedCache)
    {
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        if (_memoryCache.TryGetValue(key, out T? cachedValue) && cachedValue != null)
        {
            return cachedValue;
        }

        var distributedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(distributedValue))
        {
            return null;
        }

        var deserializedValue = JsonSerializer.Deserialize<T>(distributedValue);
        if (deserializedValue != null)
        {
            _memoryCache.Set(key, deserializedValue, _memoryExpiry);
        }

        return deserializedValue;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class
    {
        var expiryTime = expiry ?? _defaultExpiry;
        var serializedValue = JsonSerializer.Serialize(value);

        _memoryCache.Set(key, value, _memoryExpiry);

        var distributedCacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiryTime
        };

        await _distributedCache.SetStringAsync(key, serializedValue, distributedCacheOptions, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove(key);
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        if (_memoryCache is MemoryCache memCache)
        {
            var field = typeof(MemoryCache).GetField("_coherentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field?.GetValue(memCache) is object coherentState)
            {
                var entriesCollection = coherentState.GetType().GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (entriesCollection?.GetValue(coherentState) is IDictionary entries)
                {
                    var keysToRemove = entries.Keys.Cast<object>()
                        .Where(k => k.ToString()?.StartsWith(prefix) == true)
                        .ToList();

                    foreach (var key in keysToRemove)
                    {
                        _memoryCache.Remove(key);
                    }
                }
            }
        }

        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue(key, out _))
        {
            return true;
        }

        var distributedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
        return !string.IsNullOrEmpty(distributedValue);
    }
}