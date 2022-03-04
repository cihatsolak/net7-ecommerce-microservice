namespace CourseSales.Web.Services.Orders
{
    public interface IOrderService
    {
        /// <summary>
        /// Senkron iletişim- direk order mikroservisine istek yapılacak
        /// </summary>
        Task<OrderCreatedViewModel> CreateOrderAsync(CheckoutInfoInput checkoutInfoInput);

        /// <summary>
        /// Asenkron iletişim- sipariş bilgileri rabbitMQ'ya gönderilecek
        /// </summary>
        Task SuspendOrderAsync(CheckoutInfoInput checkoutInfoInput);

        Task<List<OrderViewModel>> GetOrderAsync();
    }
}
