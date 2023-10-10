using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Antilevskaya.Data;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly WEB_153501_Antilevskaya.Data.ApplicationDbContext _context;

        public EditModel(WEB_153501_Antilevskaya.Data.ApplicationDbContext context)
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

            var exhibit =  await _context.Exhibit.FirstOrDefaultAsync(m => m.Id == id);
            if (exhibit == null)
            {
                return NotFound();
            }
            Exhibit = exhibit;
           ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Exhibit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExhibitExists(Exhibit.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ExhibitExists(int id)
        {
          return (_context.Exhibit?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
