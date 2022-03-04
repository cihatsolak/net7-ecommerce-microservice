namespace CourseSales.Web.Services.Payments
{
    public interface IPaymentService
    {
        Task<bool> ReceivePaymentAsync(PaymentInfoInput paymentInfoInput);
    }
}
