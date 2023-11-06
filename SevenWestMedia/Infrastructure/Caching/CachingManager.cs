using Application.Interface;
using Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace Infrastructure.Caching;

public class CachingManager : ICacheManager
{
    private readonly IRedisConnectionProvider _redisConnectionProvider;
    private readonly ILogger<CachingManager> _logger;

    public CachingManager(
        IRedisConnectionProvider redisConnectionProvider, 
        ILogger<CachingManager> logger) 
    {
        _redisConnectionProvider = redisConnectionProvider;
        _logger = logger;

    }
    public  IEnumerable<T> GetCollectionAsync<T>(int chunkSize) where T : notnull
    {
        try
        {
            IRedisCollection<T> redisCollection = _redisConnectionProvider.RedisCollection<T>(chunkSize);

            if(!redisCollection.Any())
                return Enumerable.Empty<T>();

            return redisCollection;
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return Enumerable.Empty<T>();
        }
        return Enumerable.Empty<T>();
    }

    public async Task<IEnumerable<T>> SetCollectionAsync<T>(IEnumerable<T> value, TimeSpan cacheTime, int chunkSize) where T : notnull
    {

        IRedisCollection<T> users = _redisConnectionProvider.RedisCollection<T>(chunkSize);

        try
        {
            if (!users.Any())
            {
                _redisConnectionProvider.Connection.DropIndexAndAssociatedRecords(typeof(T));

                await _redisConnectionProvider.Connection.CreateIndexAsync(typeof(T));

                await users.InsertAsync(value, cacheTime);
            }

            if (!users.Any())
            {
                return value;
            }

        }
        catch (RedisConnectionException ex)
        {
            _logger.LogError(ex.Message);

            return value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return value;
        }
        return users;
    }

}
