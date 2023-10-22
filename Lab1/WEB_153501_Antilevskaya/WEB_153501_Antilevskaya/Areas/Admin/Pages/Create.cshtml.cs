using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Services.ExhibitService;
using WEB_153501_Antilevskaya.Services.CategoryService;

namespace WEB_153501_Antilevskaya.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IExhibitService _exhibitService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IExhibitService exhibitService, ICategoryService categoryService)
        {
            _exhibitService = exhibitService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            var categories = await _categoryService.GetCategoryListAsync();
            if (!categories.Success)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(categories.Data, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Exhibit Exhibit { get; set; } = default!;

        [BindProperty]
        public IFormFile? ExhibitImage { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Exhibit == null)
            {
                return Page();
            }
            var response = await _exhibitService.CreateExhibitAsync(Exhibit, ExhibitImage);
            if (!response.Success)
            {
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
