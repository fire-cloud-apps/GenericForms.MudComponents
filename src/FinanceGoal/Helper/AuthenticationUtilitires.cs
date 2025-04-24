using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace FinanceGoal.Helper;


public interface ISupabaseAuthService
{
    Task<SessionResponse> SignInAsync(string email, string password);
    Task SignOutAsync(string accessToken);
    Task<SessionResponse> RefreshAsync(string refreshToken);
}

public class SupabaseAuthService : ISupabaseAuthService
{
    private readonly HttpClient _http;
    private readonly string _anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImNxZmt2eXBwenJ3anJpa3lhcXN2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQ2Mjc5ODUsImV4cCI6MjA2MDIwMzk4NX0.mGgQm5SEUtDvfP8d3itpGsJqIC-5-4g2Gfnivw0ALPc";

    public SupabaseAuthService(HttpClient http) => _http = http;

    public async Task<SessionResponse> SignInAsync(string email, string password)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "token?grant_type=password")
        {
            Content = JsonContent.Create(new { email, password })
        };
        req.Headers.Add("apikey", _anonKey);
        var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<SessionResponse>();
    }

    public async Task SignOutAsync(string accessToken)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "logout");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        req.Headers.Add("apikey", _anonKey);
        await _http.SendAsync(req); // Returns 204 No Content on success
    }

    public async Task<SessionResponse> RefreshAsync(string refreshToken)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "token?grant_type=refresh_token")
        {
            Content = JsonContent.Create(new { refresh_token = refreshToken })
        };
        req.Headers.Add("apikey", _anonKey);
        var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<SessionResponse>();
    }
}

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ISupabaseAuthService _authService;
    private readonly ILocalStorageService _storage; // or ProtectedLocalStorage
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthStateProvider(ISupabaseAuthService authService, ILocalStorageService storage)
    {
        _authService = authService;
        _storage = storage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _storage.GetItemAsync<string>("access_token");
        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(_anonymous);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwt.Claims, "supabase");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(SessionResponse sess)
    {
        await _storage.SetItemAsync("access_token", sess.AccessToken);
        await _storage.SetItemAsync("refresh_token", sess.RefreshToken);
        await _storage.SetItemAsync("user_json", sess.User);
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _storage.RemoveItemAsync("access_token");
        await _storage.RemoveItemAsync("refresh_token");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}

public class SessionResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("user")]
    public JsonElement User { get; set; }
}
