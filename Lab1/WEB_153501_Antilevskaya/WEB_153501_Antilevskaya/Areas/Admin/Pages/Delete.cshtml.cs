using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Antilevskaya.Data;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly WEB_153501_Antilevskaya.Data.ApplicationDbContext _context;

        public DeleteModel(WEB_153501_Antilevskaya.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Exhibit Exhibit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Exhibit == null)
            {
                return NotFound();
            }

            var exhibit = await _context.Exhibit.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Exhibit == null)
            {
                return NotFound();
            }
            var exhibit = await _context.Exhibit.FindAsync(id);

            if (exhibit != null)
            {
                Exhibit = exhibit;
                _context.Exhibit.Remove(Exhibit);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
