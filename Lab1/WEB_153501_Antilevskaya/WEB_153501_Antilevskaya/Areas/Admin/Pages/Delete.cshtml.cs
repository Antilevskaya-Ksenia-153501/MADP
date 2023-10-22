using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Services.ExhibitService;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IExhibitService _exhibitService;

        public DeleteModel(IExhibitService exhibitService)
        {
            _exhibitService = exhibitService;
        }

        [BindProperty]
        public Exhibit Exhibit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibit = await _exhibitService.GetExhibitByIdAsync(id.Value);

            if (!exhibit.Success)
            {
                return NotFound();
            }
            else 
            {
                Exhibit = exhibit.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                await _exhibitService.DeleteExhibitAsync(id.Value);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToPage("./Index");
        }
    }
}
