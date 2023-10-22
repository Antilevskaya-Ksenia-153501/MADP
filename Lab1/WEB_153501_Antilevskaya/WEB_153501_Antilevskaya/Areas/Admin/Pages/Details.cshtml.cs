using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Services.ExhibitService;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IExhibitService _exhibitService;

        public DetailsModel(IExhibitService exhibitService)
        {
            _exhibitService = exhibitService;
        }

        public Exhibit Exhibit { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var exhibit = _exhibitService.GetExhibitByIdAsync(id.Value).Result.Data;
            if (exhibit == null)
            {
                return NotFound();
            }
            else 
            {
                Exhibit = exhibit;
            }
            return Page();
        }
    }
}
