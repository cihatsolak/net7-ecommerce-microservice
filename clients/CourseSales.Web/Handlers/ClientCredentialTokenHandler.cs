using Microsoft.Net.Http.Headers;

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
            string accessToken = await _clientCredentialTokenService.GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                throw new UnAuthorizeException();
            }

            return httpResponseMessage;
        }
    }
}
