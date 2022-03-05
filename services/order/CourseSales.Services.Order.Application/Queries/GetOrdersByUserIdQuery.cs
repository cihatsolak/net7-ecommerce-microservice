namespace CourseSales.Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<Shared.DataTransferObjects.Response<List<OrderDto>>>
    {
        public string UserId { get; set; }

        public GetOrdersByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
