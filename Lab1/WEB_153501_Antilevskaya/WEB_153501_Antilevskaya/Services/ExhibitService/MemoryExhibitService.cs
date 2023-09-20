using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Services.CategoryService;

namespace WEB_153501_Antilevskaya.Services.ExhibitService;
public class MemoryExhibitService:IExhibitService
{
    List<Exhibit> _exhibits;
    List<Category> _categories;

    public MemoryExhibitService(ICategoryService categoryService)
    {
        _categories = categoryService.GetCategoryListAsync().Result.Data;
        SetupData();
    }

    private void SetupData()
    {
        _exhibits = new List<Exhibit>()
        {
            new Exhibit{Id = 1, Title="Harmony Unveiled", Description="\"Harmony Unveiled\" is a captivating sculpture that embodies the essence of balance and unity. Crafted from gleaming bronze, the sculpture stands tall and proud, reaching a height of six feet. Its form is a harmonious composition of fluid curves and graceful lines, exuding a sense of elegance and tranquility", 
                        Category=_categories.Find(obj=>obj.NormalizedName.Equals("sculpture")), 
                        Price=100, Image="Images/Harmony Unveiled.jpg"},
            new Exhibit{Id = 2, Title="Whispering Serenity", Description="\"Whispering Serenity\" is a captivating sculpture that evokes a sense of calm and tranquility. Crafted with meticulous attention to detail, the sculpture stands as a testament to the beauty of serenity in the midst of chaos.",
                        Category=_categories.Find(obj=>obj.NormalizedName.Equals("sculpture")),
                       Price=140, Image="Images/Whispering Serenity.jpg"},
             new Exhibit{Id = 3, Title="Mystic Symphony", Description="\"Mystic Symphony\" is an enchanting painting that immerses viewers in a world of ethereal beauty and musical inspiration. The canvas comes alive with vibrant colors and flowing brushstrokes, creating a harmonious blend of tones that captivate the eye. The painting depicts a surreal landscape, where swirling melodies intertwine with nature's elements.",
                        Category=_categories.Find(obj=>obj.NormalizedName.Equals("painting")),
                        Price=230, Image="Images/Mystic Symphony.jpg"},
            new Exhibit{Id = 4, Title="Ethereal Reverie", Description="\"Ethereal Reverie\" is a captivating painting that transports viewers into a realm of dreamlike beauty and contemplation. The canvas is a tapestry of soft, pastel hues and gentle brushstrokes, evoking a sense of serenity and tranquility. The painting portrays an otherworldly scene where reality and imagination intertwine.",
                        Category=_categories.Find(obj=>obj.NormalizedName.Equals("painting")),
                       Price=180, Image="Images/Ethereal Reverie.jpg"},

        };
    }
    public Task<ResponseData<ListModel<Exhibit>>> GetExhibitListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var exhibits = new ListModel<Exhibit>();
        exhibits.Items = _exhibits.Where(obj => categoryNormalizedName == null ||
                                        obj.Category.NormalizedName.Equals(categoryNormalizedName)).ToList();
        exhibits.CurrentPage = pageNo;

        var result = new ResponseData<ListModel<Exhibit>>();
        result.Data = exhibits;
        return Task.FromResult(result);
    }
}
