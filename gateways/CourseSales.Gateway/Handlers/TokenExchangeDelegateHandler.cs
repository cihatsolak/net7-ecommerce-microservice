namespace CourseSales.Gateway.Handlers
{
    public class TokenExchangeDelegateHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public TokenExchangeDelegateHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private async Task<string> GetTokenAsync(string requestToken)
        {
            if (!string.IsNullOrEmpty(_accessToken))
                return _accessToken;

            var discoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _configuration["IdentityServerUrl"],
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discoveryDocumentResponse.IsError)
                throw discoveryDocumentResponse.Exception;

            TokenExchangeTokenRequest tokenExchangeTokenRequest = new()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                GrantType = _configuration["TokenGrantType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access-token",
                Scope = "openid discount_fullpermission payment_fullpermission"
            };

            var tokenResponse = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);
            if (tokenResponse.IsError)
                throw tokenResponse.Exception;

            _accessToken = tokenResponse.AccessToken;

            return _accessToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string bearerToken = request.Headers.Authorization.Parameter;

            string newAccessToken = await GetTokenAsync(bearerToken);
            request.SetBearerToken(newAccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
