using StackExchange.Redis;

namespace RedisExchangeApi.Web.Services
{
    public class RedisService : IRedisService
    {

        private readonly IConfiguration _configuration;
        private readonly string _redisHost;
        private readonly string _redisPort;
        public IDatabase _db;
        private ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
            _configuration = configuration;
            _redisHost = configuration["Redis:Host"]!;
            _redisPort = configuration["Redis:Port"]!;
        }

        public void Connect()
        {
            var conn = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(conn);
        }

        public IDatabase GetDb(int dbNumber = 0)
        {
            return _redis.GetDatabase(dbNumber);
        }
    }
}
