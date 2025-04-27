using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentAdminMvc.MvcProject.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
