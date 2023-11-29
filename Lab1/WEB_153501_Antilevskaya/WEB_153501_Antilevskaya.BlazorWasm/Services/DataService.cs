using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Domain.Models;


namespace WEB_153501_Antilevskaya.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly int _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;

        public DataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _pageSize = Convert.ToInt32(configuration.GetSection("ApiSettings:ItemsPerPage").Value);
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public List<Category> Categories { get; set; }
        public List<Exhibit> ObjectsList { get; set; }
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

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseResult = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Exhibit>>>(_serializerOptions);
                    ObjectsList = responseResult.Data.Items;
                    TotalPages = responseResult.Data.TotalPages;
                    CurrentPage = responseResult.Data.CurrentPage;
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
            };
        }

        public async Task<Exhibit> GetProductByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}api/exhibits/get/{id}");
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
            };
        }

        public async Task GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}api/category/");
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
            };

        }

    }
}
