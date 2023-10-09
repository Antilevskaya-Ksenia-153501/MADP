using System.Text;
using System.Text.Json;
using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;
using System.Security.Cryptography.Xml;

namespace WEB_153501_Antilevskaya.Services.ExhibitService;

public class ApiExhibitService: IExhibitService
{
    private readonly HttpClient _httpClient;
    private readonly int _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiExhibitService> _logger;
    public ApiExhibitService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiExhibitService> logger)
    {
        _httpClient = httpClient;
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
}
