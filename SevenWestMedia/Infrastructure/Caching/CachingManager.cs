using Application.Interface;
using Domain;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Infrastructure.Caching
{
    public class CachingManager : ICacheManager
    {
        private readonly IRedisConnectionProvider _redisConnectionProvider;

        public CachingManager(IRedisConnectionProvider redisConnectionProvider) 
        {
            _redisConnectionProvider = redisConnectionProvider;


        }
        public  IEnumerable<T> GetCollectionAsync<T>(int chunkSize) where T : notnull
        {
            IRedisCollection<T> redisCollection = _redisConnectionProvider.RedisCollection<T>(chunkSize);

            return redisCollection;
        }

        public async Task<IEnumerable<T>> SetCollectionAsync<T>(IEnumerable<T> value, TimeSpan cacheTime, int chunkSize) where T : notnull
        {

            IRedisCollection<T> users = _redisConnectionProvider.RedisCollection<T>(chunkSize);
        
 
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

            return users;
        }

 
    }
}
