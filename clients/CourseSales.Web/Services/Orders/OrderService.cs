namespace CourseSales.Web.Services.Orders
{
    public sealed class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(
            HttpClient httpClient, 
            IPaymentService paymentService, 
            IBasketService basketService, 
            ISharedIdentityService sharedIdentityService)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.GetAsync();

            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice
            };

            var responsePayment = await _paymentService.ReceivePaymentAsync(paymentInfoInput);
            if (!responsePayment)
                return new OrderCreatedViewModel() { Error = "Ödeme alınamadı", IsSuccessful = false };

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.UserId,
                Address = new AddressCreateInput { Province = checkoutInfoInput.Province, District = checkoutInfoInput.District, Street = checkoutInfoInput.Street, Line = checkoutInfoInput.Line, ZipCode = checkoutInfoInput.ZipCode },
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, Price = x.GetCurrentPrice, PictureUrl = "", ProductName = x.CourseName };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var httpResponseMessage = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders/saveorder", orderCreateInput);
            if (!httpResponseMessage.IsSuccessStatusCode)
                return new OrderCreatedViewModel() { Error = "Sipariş oluşturulamadı", IsSuccessful = false };

            var orderCreatedViewModel = await httpResponseMessage.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();

            orderCreatedViewModel.Data.IsSuccessful = true;
            await _basketService.DeleteAsync();

            return orderCreatedViewModel.Data;
        }

        public async Task<List<OrderViewModel>> GetOrderAsync()
        {
            var httpResponseMessage = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders/getorders");
            return httpResponseMessage.Data;
        }

        public async Task SuspendOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            throw new NotImplementedException();
        }
    }
}
