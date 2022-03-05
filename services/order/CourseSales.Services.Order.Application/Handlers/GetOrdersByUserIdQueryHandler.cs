namespace CourseSales.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Shared.DataTransferObjects.Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Shared.DataTransferObjects.Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(p => p.OrderItems).Where(p => p.BuyerId.Equals(request.UserId)).ToListAsync(cancellationToken);
            if (!orders.Any())
                return Shared.DataTransferObjects.Response<List<OrderDto>>.Fail(string.Empty, HttpStatusCode.NotFound);

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return Shared.DataTransferObjects.Response<List<OrderDto>>.Success(ordersDto, HttpStatusCode.OK);
        }
    }
}
