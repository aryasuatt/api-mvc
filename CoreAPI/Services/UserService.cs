using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CoreAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Claims
                            .FirstOrDefault(c => c.Type == "id")?.Value; // Adjust claim type if needed

            return userId;
        }
    }
}
