using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore
{
    public class EfNotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly Context _context;

        public EfNotificationRepository(Context context) : base(context)
        {
            _context = context;
        }

       
    }
}
