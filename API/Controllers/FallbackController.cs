using System.IO;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    public class FallbackController: Controller
    {
        // if this method gets call it will return index.html from wwwroot folder.
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),  "text/HTML");
        }
    }
}