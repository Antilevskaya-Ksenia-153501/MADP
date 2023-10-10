using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153501_Antilevskaya.Data;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly WEB_153501_Antilevskaya.Data.ApplicationDbContext _context;

        public CreateModel(WEB_153501_Antilevskaya.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Exhibit Exhibit { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Exhibit == null || Exhibit == null)
            {
                return Page();
            }

            _context.Exhibit.Add(Exhibit);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
