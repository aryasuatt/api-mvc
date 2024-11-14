using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CoreMVC.Models;
namespace CoreMVC.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AdminLogin", model); // Hata durumunda sayfayı tekrar göster
            }

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5219/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<LoginResponseModel>(responseContent);

                if (responseData.Token != null)
                {
                    // Token'ı saklayın
                    HttpContext.Session.SetString("AuthToken", responseData.Token);

                    // Başarılı giriş sonrası yönlendirme
                    return RedirectToAction("Index", "AdminDashboard");
                }
            }

            ModelState.AddModelError(string.Empty, "Giriş başarısız. Lütfen bilgilerinizi kontrol edin.");
            return View("AdminLogin", model);
        }

    }
}
