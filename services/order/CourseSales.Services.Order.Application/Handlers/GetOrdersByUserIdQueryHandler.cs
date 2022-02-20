using CourseSales.Services.Order.Application.Mapping;

namespace CourseSales.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(p => p.OrderItems).Where(p => p.BuyerId.Equals(request.UserId)).ToListAsync(cancellationToken);
            if (!orders.Any())
                return Response<List<OrderDto>>.Fail(string.Empty, HttpStatusCode.NotFound);

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return Response<List<OrderDto>>.Success(ordersDto, HttpStatusCode.OK);
        }
    }
}
