using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Domain.Models;

namespace WEB_153501_Antilevskaya.Services.CategoryService;
public interface ICategoryService
{ 
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();
}
