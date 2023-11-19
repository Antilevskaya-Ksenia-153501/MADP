using Microsoft.AspNetCore.Mvc;
using WEB_153501_Antilevskaya.Services.ExhibitService;
using WEB_153501_Antilevskaya.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace WEB_153501_Antilevskaya.Controllers;
public class CartController : Controller
{
    private readonly IExhibitService _exhibitService;
    private readonly Cart _cart;

    public CartController(IExhibitService exhibitService, Cart cart)
    {
        _exhibitService = exhibitService;
        _cart = cart;
    }

    [Authorize]
    [Route("[controller]/add")]
    public async Task<IActionResult> Add(int id, string returnUrl)
    {
        var data = await _exhibitService.GetExhibitByIdAsync(id);
        if (data.Success)
        {
            _cart.AddToCart(data.Data);
        }
        return Redirect(returnUrl);
    }

    [Authorize]
    [Route("[controller]/remove/{id:int}")]
    public async Task<IActionResult> Remove(int id, string returnUrl)
    {
        var data = await _exhibitService.GetExhibitByIdAsync(id);
        if (data.Success)
        {
            _cart.RemoveItems(data.Data.Id);
        }
        return Redirect(returnUrl);
    }

    public IActionResult Index()
    {
        return View(_cart);
    }
}
