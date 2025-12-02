using BlazorApp_JobPortal.Models;
using Blazored.LocalStorage;
using JobPortal.BlazorApp.Models;
using JobPortal.BlazorApp.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace JobPortal.BlazorApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> Register(RegisterDto model)
        {
            var result = await _http.PostAsJsonAsync("account/register", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> Login(LoginDto model)
        {
            var result = await _http.PostAsJsonAsync("account/login", model);

            if (!result.IsSuccessStatusCode)
                return false;

            var response = await result.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (response == null || string.IsNullOrEmpty(response.Token))
                return false;

            await _localStorage.SetItemAsync("authToken", response.Token);

            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(response.Token);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}