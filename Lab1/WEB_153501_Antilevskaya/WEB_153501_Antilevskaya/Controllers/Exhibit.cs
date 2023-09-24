using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Drawing.Printing;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Services.CategoryService;
using WEB_153501_Antilevskaya.Services.ExhibitService;

namespace WEB_153501_Antilevskaya.Controllers
{
    public class Exhibit : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IExhibitService exhibitService;
        private readonly IConfiguration configuration;

        public Exhibit(ICategoryService categoryService, IExhibitService exhibitService, IConfiguration configuration)
        {
            this.categoryService = categoryService;
            this.exhibitService = exhibitService;
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index(string? category, int page = 1)
        {
            var exhibitResponse = await exhibitService.GetExhibitListAsync(configuration, category, page);
            if (!exhibitResponse.Success)
                return NotFound(exhibitResponse.ErrorMessage);
            ViewBag.Categories = categoryService.GetCategoryListAsync().Result.Data;
            ViewData["currentCategory"] = category;
            ViewBag.previousPage = page == 1 ? 1 : page - 1;
            ViewBag.nextPage = page == exhibitResponse.Data.TotalPages ? exhibitResponse.Data.TotalPages : page + 1;
            return View(exhibitResponse.Data);
        }
        private IActionResult NotFound(object errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
