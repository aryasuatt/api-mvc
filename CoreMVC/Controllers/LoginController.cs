using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CoreMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace CoreMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        // Token'dan role bilgisi al
        private bool GetRoleFromToken(string token)
        {
            Debug.WriteLine("GetRoleFromToken metodu çalışıyor...");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken != null)
            {
                Debug.WriteLine("JWT Token başarıyla okundu.");
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role"); // "role" claim
                return roleClaim != null && roleClaim.Value == "Admin";  // "Admin" rolünü kontrol et
            }
            Debug.WriteLine("JWT Token okuma başarısız.");
            return false;  // Rol bilgisi bulunamadı veya rol "Admin" değil
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

            // API'den giriş yapmayı deniyoruz
            var response = await _httpClient.PostAsync("http://localhost:5219/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<LoginResponseModel>(responseContent);

                if (responseData.Token != null)
                {
                    // Token'ı saklıyorum
                    HttpContext.Session.SetString("AuthToken", responseData.Token);

                    // Token'dan rol bilgisini alıyoruz
                    bool isAdmin = GetRoleFromToken(responseData.Token);

                    if (isAdmin)
                    {
                        return RedirectToAction("Index", "AdminDashboard"); // Admin Dashboard'a yönlendiriyoruz
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Yetkisiz erişim: Admin kullanıcı değilsiniz.");
                        return View("AdminLogin", model);
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Giriş başarısız. Lütfen bilgilerinizi kontrol edin.");
            return View("AdminLogin", model);
        }
    }
}
