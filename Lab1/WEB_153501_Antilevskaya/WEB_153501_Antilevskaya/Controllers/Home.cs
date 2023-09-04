using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Antilevskaya.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
