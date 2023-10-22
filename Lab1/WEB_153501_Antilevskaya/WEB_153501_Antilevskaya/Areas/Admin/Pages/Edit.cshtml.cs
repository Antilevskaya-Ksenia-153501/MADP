using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Services.ExhibitService;
using WEB_153501_Antilevskaya.Services.CategoryService;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IExhibitService _exhibitService;
        private readonly ICategoryService _categoryService;

        public EditModel(IExhibitService exhibitService, ICategoryService categoryService)
        {
            _exhibitService = exhibitService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Exhibit Exhibit { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _exhibitService.GetExhibitByIdAsync(id.Value);

            if (!response.Success)
            {
                return NotFound();
            }

            var responseCategories = await _categoryService.GetCategoryListAsync();
            if (!response.Success)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(responseCategories.Data, "Id", "Name");

            Exhibit = response.Data!;

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
            try
            {
                await _exhibitService.UpdateExhibitAsync(Exhibit.Id, Exhibit, Image);
            }
            catch (Exception)
            {
                if (!await ExhibitExists(Exhibit.Id))
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

        private async Task<bool> ExhibitExists(int id)
        {
            var response = await _exhibitService.GetExhibitByIdAsync(id);
            return response.Success;
        }
    }
}
