using WEB_153501_Antilevskaya.API.Data;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Domain.Models;

namespace WEB_153501_Antilevskaya.API.Services.ExhibitService;
public class ExhibitService:IExhibitService
{
    private readonly int _maxSizePage = 20;
    private readonly AppDbContext _context;

    public ExhibitService(AppDbContext context)
    {
        _context = context;
    }
    public Task<ResponseData<ListModel<Exhibit>>> GetExhibitListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > _maxSizePage)
            pageSize = _maxSizePage;

        var query = _context.Exhibit.AsQueryable();
        var dataList = new ListModel<Exhibit>();

        query = query.Where(obj => categoryNormalizedName == null || obj.Category.NormalizedName.Equals(categoryNormalizedName));
        var count = query.Count();
        if(count == 0)
        { 
            return Task.FromResult(new ResponseData<ListModel<Exhibit>>
            {
                Data = dataList
            });
        }
        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        if (pageNo > totalPages)
        {
            return Task.FromResult(new ResponseData<ListModel<Exhibit>>
            {
                Data = null,
                Success = false,
                ErrorMessage = "No such page"
            });
        }
        dataList.Items = query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        dataList.CurrentPage = pageNo;
        dataList.TotalPages = totalPages;

        var response = new ResponseData<ListModel<Exhibit>>
        {
            Data = dataList
        };
        return Task.FromResult(response);
    }
}
