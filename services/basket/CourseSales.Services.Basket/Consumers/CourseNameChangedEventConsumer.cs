using CourseSales.Shared.Messages.Events;
using MassTransit;

namespace CourseSales.Services.Basket.Consumers
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly IRedisService _redisService;
        private readonly IBasketService _basketService;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IConfiguration _configuration;

        public CourseNameChangedEventConsumer(
            IRedisService redisService,
            IBasketService basketService,
            IConnectionMultiplexer connectionMultiplexer,
            IConfiguration configuration)
        {
            _redisService = redisService;
            _basketService = basketService;
            _connectionMultiplexer = connectionMultiplexer;
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            RedisSetting redisSetting = _configuration.GetSection(nameof(RedisSetting)).Get<RedisSetting>();

            var redisKeys = _connectionMultiplexer.GetServer(redisSetting.ConnectionString).Keys(1).Where(redisKey => Guid.TryParse(redisKey, out Guid userId)).ToList();
            if (redisKeys is null || !redisKeys.Any())
            {
                await Task.CompletedTask;
                return;
            }

            foreach (string userId in redisKeys)
            {
                bool isUpdatedBasket = false;

                var userBasket = await _basketService.GetBasketAsync(userId);
                if (!userBasket.IsSuccessful)
                    continue;

                userBasket.Data.BasketItems.ForEach(item =>
                {
                    if (item.CourseId.Equals(context.Message.CourseId))
                    {
                        item.CourseName = context.Message.UpdatedName;
                        isUpdatedBasket = true;
                    }
                });

                if (isUpdatedBasket)
                {
                    await _basketService.AddOrUpdateAsync(userBasket.Data);
                }
            }
        }
    }
}
