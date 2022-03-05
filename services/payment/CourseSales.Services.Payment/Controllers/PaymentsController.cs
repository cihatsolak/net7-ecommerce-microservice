namespace CourseSales.Services.Payment.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public PaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentRequestModel paymentRequestModel)
        {
            ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            CreateOrderMessageCommand createOrderMessageCommand = new()
            {
                BuyerId = paymentRequestModel.Order.BuyerId,
                District = paymentRequestModel.Order.Address.District,
                Line = paymentRequestModel.Order.Address.Line,
                Province = paymentRequestModel.Order.Address.Province,
                Street = paymentRequestModel.Order.Address.Street,
                ZipCode = paymentRequestModel.Order.Address.ZipCode
            };

            paymentRequestModel.Order.OrderItems.ForEach(order =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl = order.PictureUrl,
                    Price = order.Price,
                    ProductId = order.ProductId,
                    ProductName = order.ProductName
                });
            });

            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResultInstance(Shared.DataTransferObjects.Response<NoContentResponse>.Success(HttpStatusCode.OK));
        }
    }
}
