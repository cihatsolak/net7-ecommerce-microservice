namespace CourseSales.Services.Order.Application.Consumers
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            Address newAddress = new(
                province: context.Message.Province,
                district: context.Message.District,
                street: context.Message.Street,
                zipCode: context.Message.ZipCode,
                line: context.Message.Line
                );

            Domain.OrderAggregate.Order order = new Domain.OrderAggregate.Order(
                buyerId: context.Message.BuyerId,
                address: newAddress
                );

            context.Message.OrderItems.ForEach(orderItem =>
            {
                order.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.Price, orderItem.PictureUrl);
            });

            await _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
