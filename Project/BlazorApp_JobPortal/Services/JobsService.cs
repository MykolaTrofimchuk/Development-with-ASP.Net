using Blazored.LocalStorage;
using JobPortal.BlazorApp.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace JobPortal.BlazorApp.Services
{
    public class JobsService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage; 

        public JobsService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        private async Task SetAuthHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<JobDto>> GetJobs()
        {
            // await SetAuthHeader(); 
            return await _http.GetFromJsonAsync<List<JobDto>>("jobs");
        }

        public async Task<JobDto> GetJob(int id)
        {
            // await SetAuthHeader();
            return await _http.GetFromJsonAsync<JobDto>($"jobs/{id}");
        }

        public async Task CreateJob(JobDto job)
        {
            await SetAuthHeader();
            await _http.PostAsJsonAsync("jobs", job);
        }

        public async Task UpdateJob(JobDto job)
        {
            await SetAuthHeader();
            await _http.PutAsJsonAsync($"jobs/{job.Id}", job);
        }

        public async Task DeleteJob(int id)
        {
            await SetAuthHeader();
            await _http.DeleteAsync($"jobs/{id}");
        }
    }
}