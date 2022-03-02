namespace CourseSales.Web.Services.Baskets
{
    public sealed class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;

        public BasketService(
            HttpClient httpClient, 
            IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }

        public async Task AddBasketItemAsync(BasketItemViewModel basketItemViewModel)
        {
            var basket = await GetAsync();
            if (basket is not null)
            {
                if (!basket.BasketItems.Any(x => x.CourseId == basketItemViewModel.CourseId))
                {
                    basket.BasketItems.Add(basketItemViewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.BasketItems.Add(basketItemViewModel);
            }

            await SaveOrUpdateAsync(basket);
        }

        public async Task<bool> ApplyDiscountAsync(string discountCode)
        {
            await CancelApplyDiscountAsync();

            var basket = await GetAsync();
            if (basket is null)
                return false;

            var hasDiscount = await _discountService.GetDiscount(discountCode);
            if (hasDiscount is null)
                return false;

            basket.ApplyDiscount(hasDiscount.Code, hasDiscount.Rate);
            await SaveOrUpdateAsync(basket);
            return true;
        }

        public async Task<bool> CancelApplyDiscountAsync()
        {
            var basket = await GetAsync();

            if (basket is null || basket.DiscountCode is null)
                return false;

            basket.CancelDiscount();
            await SaveOrUpdateAsync(basket);

            return true;
        }

        public async Task<bool> DeleteAsync()
        {
            var httpResponseMessage = await _httpClient.DeleteAsync("baskets/delete");
            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> GetAsync()
        {
            var httpResponseMessage = await _httpClient.GetAsync("baskets/getbasket");
            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            var basketViewModel = await httpResponseMessage.Content.ReadFromJsonAsync<Response<BasketViewModel>>();
            return basketViewModel.Data;
        }

        public async Task<bool> RemoveBasketItemAsync(string courseId)
        {
            var basket = await GetAsync();
            if (basket is null)
                return false;

            var deleteBasketItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);
            if (deleteBasketItem is null)
                return false;

            var deleteResult = basket.BasketItems.Remove(deleteBasketItem);
            if (!deleteResult)
                return false;

            if (!basket.BasketItems.Any())
                basket.DiscountCode = null;

            return await SaveOrUpdateAsync(basket);
        }

        public async Task<bool> SaveOrUpdateAsync(BasketViewModel basketViewModel)
        {
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets/addorupdate", basketViewModel);
            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
}
