namespace CourseSales.Services.Order.Application.Consumers
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        public CourseNameChangedEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItems = await _orderDbContext.OrderItems.Where(p => p.ProductId.Equals(context.Message.CourseId)).ToListAsync();
            if (!orderItems.Any())
            {
                await Task.CompletedTask;
                return;
            }

            orderItems.ForEach(orderItem =>
            {
                orderItem.UpdateOrderItem(context.Message.UpdatedName, orderItem.PictureUrl, orderItem.Price);
            });

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
