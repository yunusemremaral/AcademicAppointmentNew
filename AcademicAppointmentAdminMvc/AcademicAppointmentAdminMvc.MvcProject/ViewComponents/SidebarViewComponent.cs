﻿using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentAdminMvc.MvcProject.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
