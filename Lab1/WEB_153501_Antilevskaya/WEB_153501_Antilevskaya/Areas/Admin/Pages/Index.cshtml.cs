using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Services.ExhibitService;
using Microsoft.AspNetCore.Mvc;
using WEB_153501_Antilevskaya.Extensions;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages;
public class IndexModel : PageModel
{
    private readonly IExhibitService _exhibitService;
    public IList<Exhibit> Exhibit { get; set; }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public IndexModel(IExhibitService exhibitService)
    {
        _exhibitService = exhibitService;
    }
    public async Task<IActionResult> OnGetAsync(string? category)
    {
        Exhibit = _exhibitService.GetExhibitListAsync(category, CurrentPage).Result.Data.Items;

        if (Request.IsAjaxRequest())
        {
            return Partial("_ExhibitListPartial", new
            {
                CurrentPage,
                TotalPages,
                Exhibit,
                IsAdmin = true,
                ReturnUrl = Request.Path + Request.QueryString.ToUriComponent()
            });
        }
        return Page();

        //ViewData["previousPage"] = CurrentPage == 1 ? 1 : CurrentPage - 1;
        //ViewData["nextPage"] = CurrentPage == Exhibit.TotalPages ? Exhibit.TotalPages : CurrentPage + 1;
    }
}
