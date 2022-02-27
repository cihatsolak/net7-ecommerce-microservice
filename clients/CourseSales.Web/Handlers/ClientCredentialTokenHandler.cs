namespace CourseSales.Web.Handlers
{
    public class ClientCredentialTokenHandler : DelegatingHandler
    {
        private readonly IClientCredentialTokenService _clientCredentialTokenService;

        public ClientCredentialTokenHandler(IClientCredentialTokenService clientCredentialTokenService)
        {
            _clientCredentialTokenService = clientCredentialTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _clientCredentialTokenService.GetTokenAsync());

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                throw new UnAuthorizeException();
            }

            return httpResponseMessage;
        }
    }
}
