using System.Net.Http.Headers;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class JwtCookieHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtCookieHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Cookie'den JWT token'ı alıyoruz
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];

            if (!string.IsNullOrEmpty(token))
            {
                // Token varsa, Authorization başlığına ekliyoruz
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // Request'i göndermek
            return await base.SendAsync(request, cancellationToken);
        }
    }


}
