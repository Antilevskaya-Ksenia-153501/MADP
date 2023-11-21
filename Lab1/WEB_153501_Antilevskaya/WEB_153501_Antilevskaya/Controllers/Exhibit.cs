using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Drawing.Printing;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Extensions;
using WEB_153501_Antilevskaya.Services.CategoryService;
using WEB_153501_Antilevskaya.Services.ExhibitService;

namespace WEB_153501_Antilevskaya.Controllers;

public class ExhibitController : Controller
{
    private readonly ICategoryService categoryService;
    private readonly IExhibitService exhibitService;

    public ExhibitController(ICategoryService categoryService, IExhibitService exhibitService)
    {
        this.categoryService = categoryService;
        this.exhibitService = exhibitService;
    }

    public async Task<IActionResult> Index(string? category, int page = 1)
    {
        if (category == "All")
            category = null;
        var exhibitResponse = await exhibitService.GetExhibitListAsync(category, page);
        if (!exhibitResponse.Success)
            return NotFound(exhibitResponse.ErrorMessage);
        var categoriesResponse = await categoryService.GetCategoryListAsync();
        if (!categoriesResponse.Success)
            return NotFound(categoriesResponse.ErrorMessage);
        ViewBag.Categories = categoriesResponse.Data;
        if (category != null)
            ViewData["currentCategory"] = category;
        else
            ViewData["currentCategory"] = "All";
        ViewBag.previousPage = page == 1 ? 1 : page - 1;
        ViewBag.nextPage = page == exhibitResponse.Data.TotalPages ? exhibitResponse.Data.TotalPages : page + 1;
        //var temp = exhibitResponse.Data;
        ViewData["currentPage"] = exhibitResponse.Data.CurrentPage;
        ViewData["totalPages"] = exhibitResponse.Data.TotalPages;

        if (Request.IsAjaxRequest())
        {
            return PartialView("_ExhibitListPartial", new
            {
                Exhibits = exhibitResponse.Data,
                Category = category,
                CurrentCategory = ViewData["currentCategory"],
                CurrentPage = exhibitResponse.Data.CurrentPage,
                TotalPages = exhibitResponse.Data.TotalPages,
                ReturnUrl = Request.Path + Request.QueryString.ToUriComponent(),
                IsAdmin = false
            });

        }
        return View(exhibitResponse.Data.Items);
    }
    private IActionResult NotFound(object? errorMessage)
    {
        return new NotFoundObjectResult(errorMessage);
    }
}
