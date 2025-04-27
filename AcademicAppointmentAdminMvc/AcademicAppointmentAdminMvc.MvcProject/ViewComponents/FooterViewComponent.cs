using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentAdminMvc.MvcProject.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
