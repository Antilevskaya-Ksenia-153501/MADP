using Microsoft.AspNetCore.Mvc;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Services.CategoryService;
using WEB_153501_Antilevskaya.Services.ExhibitService;

namespace WEB_153501_Antilevskaya.Controllers
{
    public class Exhibit: Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IExhibitService exhibitService;
        
        public Exhibit(ICategoryService categoryService, IExhibitService exhibitService)
        {
            this.categoryService = categoryService;
            this.exhibitService = exhibitService;
        }
        public async Task<IActionResult> Index()
        {
            var exhibitResponse = await exhibitService.GetExhibitListAsync("", 1);
            if (!exhibitResponse.Success)
                return NotFound(exhibitResponse.ErrorMessage);
            return View(exhibitResponse.Data.Items);
        }
        private IActionResult NotFound(object errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
