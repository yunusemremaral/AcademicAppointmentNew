using System.Net.Http.Headers;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    public class JwtCookieHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtCookieHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
            if (token != null)
            {
                request.Headers.Add("Authorization", "Bearer " + token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }


}
