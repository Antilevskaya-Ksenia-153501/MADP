using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.API.Data;

namespace WEB_153501_Antilevskaya.API.Services.CategoryService;
public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }
    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var categories = _context.Category.ToList();
        var result = new ResponseData<List<Category>>();
        result.Data = categories;
        return Task.FromResult(result);
    }
}
