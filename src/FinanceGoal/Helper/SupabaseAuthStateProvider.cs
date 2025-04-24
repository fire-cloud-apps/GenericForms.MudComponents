using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace FinanceGoal.Helper;


public class SupabaseAuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IJSRuntime _jsRuntime;

    public SupabaseAuthStateProvider(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime)
    {
        try
        {
            _httpClientFactory = httpClientFactory;
            _jsRuntime = jsRuntime;
        }
        catch(Exception ex) 
        {
            Console.WriteLine(ex);

        }
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var accessToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "supabase_access_token");
        if (string.IsNullOrEmpty(accessToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var httpClient = _httpClientFactory.CreateClient("SupabaseAuth");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.GetAsync("user");
        if (response.IsSuccessStatusCode)
        {
            var userJson = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(userJson);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id),
                new Claim(ClaimTypes.Email, user.email)
            };

            if (user.user_metadata != null && user.user_metadata.TryGetValue("role", out var role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var identity = new ClaimsIdentity(claims, "Supabase");
            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationState(principal);
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Try to refresh token
            var refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "supabase_refresh_token");
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var refreshContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", refreshToken)
                });

                var refreshResponse = await httpClient.PostAsync("token", refreshContent);
                if (refreshResponse.IsSuccessStatusCode)
                {
                    var newTokenResponse = await refreshResponse.Content.ReadFromJsonAsync<TokenResponse>();
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "supabase_access_token", newTokenResponse.access_token);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "supabase_refresh_token", newTokenResponse.refresh_token);

                    // Retry with new access token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newTokenResponse.access_token);
                    response = await httpClient.GetAsync("user");
                    if (response.IsSuccessStatusCode)
                    {
                        var userJson = await response.Content.ReadAsStringAsync();
                        var user = JsonSerializer.Deserialize<User>(userJson);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.id),
                            new Claim(ClaimTypes.Email, user.email)
                        };

                        if (user.user_metadata != null && user.user_metadata.TryGetValue("role", out var role))
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        var identity = new ClaimsIdentity(claims, "Supabase");
                        var principal = new ClaimsPrincipal(identity);
                        return new AuthenticationState(principal);
                    }
                }
            }

            // If refresh fails, remove tokens
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "supabase_access_token");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "supabase_refresh_token");
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task LoginAsync(string email, string password)
    {
        var httpClient = _httpClientFactory.CreateClient("SupabaseAuth");
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("email", email),
            new KeyValuePair<string, string>("password", password)
        });

        var response = await httpClient.PostAsync("token", content);
        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "supabase_access_token", tokenResponse.access_token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "supabase_refresh_token", tokenResponse.refresh_token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        else
        {
            throw new Exception("Login failed");
        }
    }

    public async Task LogoutAsync()
    {
        var accessToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "supabase_access_token");
        if (!string.IsNullOrEmpty(accessToken))
        {
            var httpClient = _httpClientFactory.CreateClient("SupabaseAuth");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            await httpClient.PostAsync("logout", null);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "supabase_access_token");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "supabase_refresh_token");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public async Task SignUpAsync(string email, string password, Dictionary<string, object> userMetadata = null)
    {
        var httpClient = _httpClientFactory.CreateClient("SupabaseAuth");
        var signUpData = new
        {
            email = email,
            password = password,
            user_metadata = userMetadata
        };

        var content = JsonContent.Create(signUpData);
        var response = await httpClient.PostAsync("signup", content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Sign-up failed");
        }
    }
}

public class User
{
    public string id { get; set; }
    public string aud { get; set; }
    public string role { get; set; }
    public string email { get; set; }
    [JsonPropertyName("user_metadata")]
    public Dictionary<string, object> user_metadata { get; set; }
}

public class TokenResponse
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
}
