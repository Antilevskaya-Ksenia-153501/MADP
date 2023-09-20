using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.Services.CategoryService;
public class MemoryCategoryService : ICategoryService
{
    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var categories = new List<Category>()
        {
            new Category{Id = 1, Name="Sculpture", NormalizedName="sculpture" },
            new Category{Id = 2, Name="Painting", NormalizedName="painting"}
        };
        var result = new ResponseData<List<Category>>();
        result.Data = categories;
        return Task.FromResult(result);
    }
}
