using ClientMM.Interfaces;
using Microsoft.JSInterop;

namespace ClientMM.Services
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJSRuntime _jSRuntime;

        public CookieService(IHttpContextAccessor httpContextAccessor, IJSRuntime jSRuntime)
        {
            _httpContextAccessor = httpContextAccessor;
            _jSRuntime = jSRuntime;
        }

        public async Task SetCookie(string name, string value)
        {
            await _jSRuntime.InvokeVoidAsync("eval",
                $"document.cookie = '{name}={value}; path=/; max-age={1 * 86400}; SameSite=Lax; Secure'");
        }

        public string GetCookie(string name)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Request.Cookies.TryGetValue(name, out var value))
            {
                return value;
            }
            return string.Empty;
        }
    }
}
