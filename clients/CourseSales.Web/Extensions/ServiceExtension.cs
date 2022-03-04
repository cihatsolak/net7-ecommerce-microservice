using CourseSales.Web.Services.Orders;

namespace CourseSales.Web.Extensions
{
    public static class ServiceExtension
    {
        public static void AddConfigures(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetRequiredSection(nameof(ServiceApiSettings)));
            builder.Services.Configure<ClientSettings>(builder.Configuration.GetRequiredSection(nameof(ClientSettings)));
        }

        public static void AddHttpClientServices(this WebApplicationBuilder builder)
        {
            var serviceApiSettings = builder.Configuration.GetSection(nameof(ServiceApiSettings)).Get<ServiceApiSettings>();

            builder.Services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();
            builder.Services.AddHttpClient<IIdentityService, IdentityService>();

            builder.Services.AddHttpClient<IUserService, UserService>(options =>
            {
                options.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            builder.Services.AddHttpClient<ICatalogService, CatalogService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            builder.Services.AddHttpClient<IPhotoStockService, PhotoStockService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            builder.Services.AddHttpClient<IBasketService, BasketService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}{serviceApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            builder.Services.AddHttpClient<IDiscountService, DiscountService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}{serviceApiSettings.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            builder.Services.AddHttpClient<IPaymentService, PaymentService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}{serviceApiSettings.Payment.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            builder.Services.AddHttpClient<IOrderService, OrderService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}{serviceApiSettings.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }
    }
}
