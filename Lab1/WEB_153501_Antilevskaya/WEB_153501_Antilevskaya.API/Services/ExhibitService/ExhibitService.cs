using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WEB_153501_Antilevskaya.API.Data;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Domain.Models;

namespace WEB_153501_Antilevskaya.API.Services.ExhibitService;
public class ExhibitService : IExhibitService
{
    private readonly int _maxSizePage = 20;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExhibitService(AppDbContext context, IWebHostEnvironment env, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _env = env;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    public Task<ResponseData<ListModel<Exhibit>>> GetExhibitListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > _maxSizePage)
            pageSize = _maxSizePage;

        var query = _context.Exhibit.AsQueryable();
        var dataList = new ListModel<Exhibit>();

        query = query.Where(obj => categoryNormalizedName == null || obj.Category.NormalizedName.Equals(categoryNormalizedName));
        var count = query.Count();
        if (count == 0)
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

    public async Task<ResponseData<Exhibit>> GetExhibitByIdAsync(int id)
    {
        var exhibit = await _context.Exhibit.FindAsync(id);
        if (exhibit == null)
        {
            return new ResponseData<Exhibit>
            {
                Data = null,
                Success = false,
                ErrorMessage = "No such exhibit"
            };
        }
        return new ResponseData<Exhibit>()
        {
            Data = exhibit
        };
    }
    public async Task UpdateExhibitAsync(int id, Exhibit item)
    {
        var exhibit = await _context.Exhibit.FindAsync(id);
        if (exhibit == null)
        {
            throw new ArgumentException("There is no such exhibit");
        }
        exhibit.Title = item.Title;
        exhibit.Description = item.Description;
        exhibit.Price = item.Price;
        exhibit.CategoryId = item.CategoryId;
        exhibit.Category = item.Category;
        _context.Entry(exhibit).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task TaskDeleteExhibitAsync(int id)
    {
        var exhibit = _context.Exhibit.FindAsync(id);
        if (exhibit == null)
        {
            throw new ArgumentException("There is no such exhibit");
        }
        _context.Remove(exhibit);
        await _context.SaveChangesAsync();
    }
    public async Task<ResponseData<Exhibit>> CreateExhibitAsync(Exhibit exhibit)
    {
        _context.Exhibit.Add(exhibit);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ResponseData<Exhibit>
            {
                Data = null,
                Success = false,
                ErrorMessage = ex.Message
            };
        }
        return new ResponseData<Exhibit>()
        {
            Data = exhibit
        };
    }
    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        var responseData = new ResponseData<string>();
        var exhibit = await _context.Exhibit.FindAsync(id);
        if (exhibit == null)
        {
            responseData.Success = false;
            responseData.ErrorMessage = "There is no such exhibit";
            responseData.Data = null;
            return responseData;
        }
        var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
        var imageFolder = Path.Combine(_env.WebRootPath, "Images");
        if (formFile != null)
        {
            if (!String.IsNullOrEmpty(exhibit.Image))
            {
                var prevImage = Path.GetFileName(exhibit.Image);
            }
            var ext = Path.GetExtension(formFile.FileName);
            var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
            var filePath = Path.Combine(imageFolder, fName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            _context.Entry(exhibit).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            exhibit.Image = $"{host}/Images/{fName}";
            await _context.SaveChangesAsync();
        }
        responseData.Data = exhibit.Image;
        return responseData;
    }
}
