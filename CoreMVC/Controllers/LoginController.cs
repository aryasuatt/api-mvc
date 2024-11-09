using Microsoft.AspNetCore.Mvc;

namespace CoreMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Admin_Login()
        {
            return View();
        }
    }
}
