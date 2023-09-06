using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153501_Antilevskaya.Models;

namespace WEB_153501_Antilevskaya.Controllers
{
    public class Home : Controller
    {
        IEnumerable<ListDemo> listExample = new List<ListDemo>()
        {
            new ListDemo(1, "Item 1"),
            new ListDemo(2, "Item 2"),
            new ListDemo(3, "Item 3")
        };
        public IActionResult Index()
        {
            ViewData["TitlePage"] = "Лабораторная работа 2";
            ViewBag.ListExample = new SelectList(listExample, "Id", "Name");
            return View();
        }
    }
}
