namespace CourseSales.Services.Basket.Services.Redis
{
    public interface IRedisService
    {
        public IDatabase Database { get; set; }
    }

    public class RedisService : IRedisService
    {
        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            Database = connectionMultiplexer.GetDatabase(1);
        }

        public IDatabase Database { get; set; }
    }
}
