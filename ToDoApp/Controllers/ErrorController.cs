using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorPage(int code)
        {
            return View();
        }
    }
}
