namespace CourseSales.Web.Services.Discounts
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscountAsync(string discountCode)
        {
            var httpResponseMessage = await _httpClient.GetAsync($"discounts/getbycode/{discountCode}");
            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            var discount = await httpResponseMessage.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();
            return discount.Data;
        }
    }
}
