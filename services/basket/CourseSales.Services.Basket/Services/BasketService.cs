namespace CourseSales.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<bool>> AddOrUpdateAsync(BasketModel basketModel);
        Task<Response<bool>> DeleteAsync(string userId);
        Task<Response<BasketModel>> GetBasketAsync(string userId);
    }

    public sealed class BasketService : IBasketService
    {
        private readonly IRedisService _redisService;

        public BasketService(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<BasketModel>> GetBasketAsync(string userId)
        {
            var basket = await _redisService.Database.StringGetAsync(userId);
            if (!basket.HasValue)
                return Response<BasketModel>.Fail("Basket not found!", HttpStatusCode.NotFound);

            var basketResponseModel = JsonSerializer.Deserialize<BasketModel>(basket);
            return Response<BasketModel>.Success(basketResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<bool>> AddOrUpdateAsync(BasketModel basketModel)
        {
            var basketSerializeModel = JsonSerializer.Serialize(basketModel);
            bool succedeed = await _redisService.Database.StringSetAsync(basketModel.UserId, basketSerializeModel);
            if (succedeed)
                return Response<bool>.Success(HttpStatusCode.NoContent);

            return Response<bool>.Fail("Operation failed.", HttpStatusCode.InternalServerError);
        }

        public async Task<Response<bool>> DeleteAsync(string userId)
        {
            bool succedeed = await _redisService.Database.KeyDeleteAsync(userId);
            if (succedeed)
                return Response<bool>.Success(HttpStatusCode.NoContent);

            return Response<bool>.Fail("Basket not found.", HttpStatusCode.NotFound);
        }
    }
}
