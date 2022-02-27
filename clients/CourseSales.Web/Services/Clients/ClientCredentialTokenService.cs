namespace CourseSales.Web.Services.Clients
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public ClientCredentialTokenService(
            IOptions<ServiceApiSettings> serviceApiSettings, 
            IOptions<ClientSettings> clientSettings, 
            IClientAccessTokenCache clientAccessTokenCache, 
            HttpClient httpClient)
        {
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            ClientAccessToken clientAccessToken = await _clientAccessTokenCache.GetAsync("WebClientToken", default);
            if (clientAccessToken is not null)
            {
                return clientAccessToken.AccessToken;
            }

            var discoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discoveryDocumentResponse.IsError)
            {
                throw discoveryDocumentResponse.Exception;
            }

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address = discoveryDocumentResponse.TokenEndpoint
            };

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }

            await _clientAccessTokenCache.SetAsync("WebClientToken", tokenResponse.AccessToken, tokenResponse.ExpiresIn, default);

            return tokenResponse.AccessToken;
        }
    }
}
