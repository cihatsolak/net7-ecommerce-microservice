namespace CourseSales.Services.Basket.Controllers
{
    public class BasketsController : BaseApiController
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IBasketService _basketService;

        public BasketsController(
            IBasketService basketService, 
            ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            string userId = _sharedIdentityService.UserId;
            var basketModel = await _basketService.GetBasketAsync(userId);
            return CreateActionResultInstance(basketModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(BasketModel basketModel)
        {
            var result = await _basketService.AddOrUpdateAsync(basketModel);
            return CreateActionResultInstance(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            string userId = _sharedIdentityService.UserId;
            var result = await _basketService.DeleteAsync(userId);
            return CreateActionResultInstance(result);
        }
    }
}
