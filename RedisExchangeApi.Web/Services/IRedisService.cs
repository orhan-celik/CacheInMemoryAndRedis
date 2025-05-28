using StackExchange.Redis;

namespace RedisExchangeApi.Web.Services
{
    public interface IRedisService
    {
        void Connect();

        IDatabase GetDb(int dbNumber = 0);
    }
}
