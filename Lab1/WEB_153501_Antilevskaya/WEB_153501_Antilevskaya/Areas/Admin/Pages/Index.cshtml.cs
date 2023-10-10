using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Services.ExhibitService;
using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages;
public class IndexModel : PageModel
{
    private readonly IExhibitService _exhibitService;
    public ListModel<Exhibit> Exhibit { get; set; }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public IndexModel(IExhibitService exhibitService)
    {
        _exhibitService = exhibitService;
    }
    public async Task OnGetAsync(string? category)
    {
        Exhibit = _exhibitService.GetExhibitListAsync(category, CurrentPage).Result.Data;
        ViewData["previousPage"] = CurrentPage == 1 ? 1 : CurrentPage - 1;
        ViewData["nextPage"] = CurrentPage == Exhibit.TotalPages ? Exhibit.TotalPages : CurrentPage + 1;
    }
}
