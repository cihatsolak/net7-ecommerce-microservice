namespace CourseSales.Web.Services.Orders
{
    public sealed class OrderService : IOrderService
    {
        public Task<OrderCreatedViewModel> CreateOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderViewModel>> GetOrderAsync()
        {
            throw new NotImplementedException();
        }

        public Task SuspendOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            throw new NotImplementedException();
        }
    }
}
