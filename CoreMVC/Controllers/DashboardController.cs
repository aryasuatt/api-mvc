using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreMVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Kullanıcının IsAdmin özelliğini doğrula
            var isAdminClaim = User.FindFirst("IsAdmin")?.Value;

            // IsAdmin özelliğine göre dashboard yönlendirmesi yap
            if (bool.TryParse(isAdminClaim, out bool isAdmin) && isAdmin)
            {

                return RedirectToAction("AdminDashboard");
            }
            else
            {

                return RedirectToAction("UserDashboard");
            }
        }

        public IActionResult AdminDashboard()
        {
            return View("AdminDashboard"); 
        }

        public IActionResult UserDashboard()
        {
            return View("UserDashboard"); 
        }
    }
}
