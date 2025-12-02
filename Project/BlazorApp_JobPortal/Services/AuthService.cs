using Blazored.LocalStorage;
using JobPortal.BlazorApp.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using BlazorApp_JobPortal.Models;
using JobPortal.BlazorApp.Models;

namespace JobPortal.BlazorApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> Register(RegisterDto registerModel)
        {
            var result = await _http.PostAsJsonAsync("account/register", registerModel);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> Login(LoginDto loginModel)
        {
            var result = await _http.PostAsJsonAsync("account/login", loginModel);

            if (!result.IsSuccessStatusCode) return false;

            var response = await result.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (response == null || string.IsNullOrEmpty(response.Token)) return false;

            await _localStorage.SetItemAsync("authToken", response.Token);

            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(response.Token);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }

    public class AuthResponseDto { public string Token { get; set; } }
}