namespace CourseSales.Web.Services.Identity
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ClientSettings> clientSettings,
            IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshTokenAsync()
        {
            var discoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discoveryDocumentResponse.IsError)
                throw discoveryDocumentResponse.Exception;

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = discoveryDocumentResponse.TokenEndpoint
            };

            var tokenResponse = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (tokenResponse.IsError)
                return null;

            List<AuthenticationToken> authenticationTokens = new()
            {
                new AuthenticationToken 
                { 
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                { 
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                { 
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                }
            };

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            var authenticationProperties = authenticationResult.Properties;
            authenticationProperties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, authenticationProperties);

            return tokenResponse;
        }

        public async Task RevokeRefreshTokenAsync()
        {
            var discoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discoveryDocumentResponse.IsError)
                throw discoveryDocumentResponse.Exception;

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = discoveryDocumentResponse.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = OpenIdConnectParameterNames.RefreshToken
            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignInAsync(SigninInput signinInput)
        {
            var discoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discoveryDocumentResponse.IsError)
                throw discoveryDocumentResponse.Exception;

            PasswordTokenRequest passwordTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signinInput.Email,
                Password = signinInput.Password,
                Address = discoveryDocumentResponse.TokenEndpoint
            };

            var tokenResponse = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
            if (tokenResponse.IsError)
            {
                var responseContent = await tokenResponse.HttpResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Response<bool>.Fail(errorResponse.Errors, HttpStatusCode.BadRequest);
            }

            UserInfoRequest userInfoRequest = new()
            {
                Token = tokenResponse.AccessToken,
                Address = discoveryDocumentResponse.UserInfoEndpoint
            };

            var userInfoResponse = await _httpClient.GetUserInfoAsync(userInfoRequest);
            if (userInfoResponse.IsError)
                throw userInfoResponse.Exception;

            ClaimsIdentity claimsIdentity = new(userInfoResponse.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            AuthenticationProperties authenticationProperties = new();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
                }
            });

            authenticationProperties.IsPersistent = signinInput.IsRemember;
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(HttpStatusCode.OK);
        }
    }
}
