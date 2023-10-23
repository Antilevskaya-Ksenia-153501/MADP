using System.Text;
using System.Text.Json;
using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;
using System.Net.Http.Headers;
using Azure.Core;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace WEB_153501_Antilevskaya.Services.ExhibitService;

public class ApiExhibitService: IExhibitService
{
    private readonly HttpClient _httpClient;
    private readonly int _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiExhibitService> _logger;
    private readonly HttpContext _httpContext;
    public ApiExhibitService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiExhibitService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContext = httpContextAccessor.HttpContext;
        _pageSize = configuration.GetValue<int>("ItemsPerPage");
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }

    public async Task<ResponseData<ListModel<Exhibit>>> GetExhibitListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}exhibits/");
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
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Exhibit>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return new ResponseData<ListModel<Exhibit>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };
            }
        }
        _logger.LogError($"-----> Данные не получены от сервера. Error: { response.StatusCode.ToString()}");
        return new ResponseData<ListModel<Exhibit>>
        {
            Success = false,
            ErrorMessage = $"Данные не получены от сервера. Error:{ response.StatusCode.ToString() }"
        };
    }

    public async Task<ResponseData<Exhibit>> GetExhibitByIdAsync(int id)

    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}exhibits/get/{id}");
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Exhibit>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return new ResponseData<Exhibit>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };
            }
        }
        _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        return new ResponseData<Exhibit>
        {
            Success = false,
            ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode.ToString()}"
        };
    }
    public async Task DeleteExhibitAsync(int id)
    {
        var uriString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}exhibits/delete/{id}");
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _httpClient.DeleteAsync(new Uri(uriString.ToString()));

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        }
    }

    public async Task UpdateExhibitAsync(int id, Exhibit exhibit, IFormFile? formFile)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}exhibits/update/{id}");
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _httpClient.PutAsync(new Uri(urlString.ToString()),
            new StringContent(JsonSerializer.Serialize(exhibit), Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            if (formFile is not null)
            {
                //int exhibitId = (await response.Content.ReadFromJsonAsync<ResponseData<Exhibit>>(_serializerOptions)).Data.Id;
                await SaveImageAsync(id, formFile);
            }
        }
        else
        {
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        }
    }

    public async Task<ResponseData<Exhibit>> CreateExhibitAsync(Exhibit exhibit, IFormFile? formFile)
    {
        var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}exhibits/create");
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _httpClient.PostAsJsonAsync(uri, exhibit, _serializerOptions);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Exhibit>>(_serializerOptions);
            if (formFile is not null)
            {
                await SaveImageAsync(data.Data.Id, formFile);
            }
            return data;
        }
        _logger.LogError($"-----> Объект не добавлен. Error:{response.StatusCode}");
        return new ResponseData<Exhibit>
        {
            Success = false,
            ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode}"
        };
    }
    private async Task SaveImageAsync(int id, IFormFile image)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}exhibits/image/{id}")
        };
        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(image.OpenReadStream());
        content.Add(streamContent, "formFile", image.FileName);
        request.Content = content;
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        await _httpClient.SendAsync(request);
    }

}
