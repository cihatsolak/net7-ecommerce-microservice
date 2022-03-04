namespace CourseSales.Web.Services.Payments
{
    public sealed class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReceivePaymentAsync(PaymentInfoInput paymentInfoInput)
        {
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<PaymentInfoInput>("payments/receivepayment", paymentInfoInput);
            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
