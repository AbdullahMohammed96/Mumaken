using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.ContactUsInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Implementation.ContactUsImplementation
{
    public class ContactUsServices : IContactUsServices
    {
        private readonly ApplicationDbContext _context;

        public ContactUsServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.Entities.SettingTables.ContactUs>> GetContactUs()
        {
            return await _context.ContactUs.OrderByDescending(c => c.Id).ToListAsync();
        }

        public async Task<bool> DeleteContactUs(int? id)
        {
            var contact = await _context.ContactUs.FindAsync(id);
            contact.IsDeleted = true;
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
