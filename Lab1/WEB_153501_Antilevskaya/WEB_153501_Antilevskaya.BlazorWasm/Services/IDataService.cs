using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.BlazorWasm.Services
{
    public interface IDataService
    {
        List<Category> Categories { get; set; }
        List<Exhibit> ObjectsList { get; set; }

        bool Success { get; set; }
        string ErrorMessage { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }

        public Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);

        public Task<Exhibit> GetProductByIdAsync(int id);

        public Task GetCategoryListAsync();
    }
}
