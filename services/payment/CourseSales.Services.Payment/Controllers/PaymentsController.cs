namespace CourseSales.Services.Payment.Controllers
{
    public class PaymentsController : BaseApiController
    {
        [HttpPost]
        public IActionResult ReceivePayment(PaymentRequestModel aymentRequestModel)
        {
            return CreateActionResultInstance(Response<NoContentResponse>.Success(HttpStatusCode.OK));
        }
    }
}
