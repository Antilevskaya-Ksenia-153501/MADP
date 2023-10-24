using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WEB_153501_Antilevskaya.IdentityServer.Models;

namespace WEB_153501_Antilevskaya.IdentityServer.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        public AvatarController(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment) 
        {
            _userManager = userManager;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var id = _userManager.GetUserId(User);
            var avatarFileName = id + ".png";
            var filePath = Path.Combine(_environment.ContentRootPath, "Images", avatarFileName);
            if (System.IO.File.Exists(filePath))
            {
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(avatarFileName, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                var fileContents = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileContents, contentType);
            }
            else
            {
                var defaultFile = Path.Combine(_environment.ContentRootPath, "Images", "default_avatar.png");
                var defaultContent = await System.IO.File.ReadAllBytesAsync(defaultFile);
                return File(defaultContent, "image/png");
            }
        }
    }
}
