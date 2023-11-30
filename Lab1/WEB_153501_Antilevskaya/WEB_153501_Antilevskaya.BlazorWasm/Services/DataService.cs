using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;


namespace WEB_153501_Antilevskaya.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly int _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly IAccessTokenProvider _tokenProvider;

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _pageSize = Convert.ToInt32(configuration.GetSection("ApiSettings:ItemsPerPage").Value);
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _tokenProvider = tokenProvider;
        }

        public event Action DataLoaded;
        public List<Category> Categories { get; set; }
        public List<Exhibit>? ObjectsList { get; set; }
        public bool Success { get; set; } = true; 
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public async Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}api/exhibits/");
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            }
            if (pageNo > 1)
            {
                urlString.Append($"{pageNo}");
            }
            if (!_pageSize.Equals(3))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
            }
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseResult = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Exhibit>>>(_serializerOptions);
                        ObjectsList = responseResult.Data.Items;
                        TotalPages = responseResult.Data.TotalPages;
                        CurrentPage = responseResult.Data.CurrentPage;
                        OnDataLoaded();
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode.ToString()}";
                }
            }
        }

        public async Task<Exhibit> GetProductByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}api/exhibits/get/{id}");
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        return (await response.Content.ReadFromJsonAsync<ResponseData<Exhibit>>(_serializerOptions)).Data;
                    }
                    catch (JsonException ex)
                    {

                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                        return null;
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode.ToString()}";
                    return null;
                }
            }
            return null;
        }

        public async Task GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}api/category/");
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseResult = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
                        Categories = responseResult.Data;
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode.ToString()}";
                }
            }

        }
        private void OnDataLoaded()
        {
            DataLoaded?.Invoke();
        }
    }
}
