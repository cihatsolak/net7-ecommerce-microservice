namespace CourseSales.Services.Payment.Controllers
{
    public class FakePaymentsController : BaseApiController
    {
        [HttpPost]
        public IActionResult ReceivePayment()
        {
            return CreateActionResultInstance(Response<NoContentResponse>.Success(HttpStatusCode.OK));
        }
    }
}
