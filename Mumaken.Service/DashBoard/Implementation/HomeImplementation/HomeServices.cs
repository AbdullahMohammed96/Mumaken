using AAITHelper.Enums;
using Mumaken.Domain.Enums;
using Mumaken.Domain.Model;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.HomeInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Implementation.HomeImplementation
{
    public class HomeServices : IHomeServices
    {
        private readonly ApplicationDbContext _context;

        public HomeServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public DashBoardHomeModel HomeIndex()
        {
            var data = (from st in _context.Settings
                        let UserCount = _context.Users.Where(x => x.TypeUser != UserType.Admin.ToNumber() && !x.IsDeleted).Count()
                        let DriverCount = _context.Users.Where(x => x.TypeUser == UserType.Client.ToNumber() && (x.AddedUserType == 1 || x.AddedUserType == 2) && !x.IsDeleted).Count()
                        let IndependentDriversCount = _context.Users.Where(x => x.TypeUser == UserType.Client.ToNumber() && x.AddedUserType == (int)AddedUserType.AddByMobile && !x.IsDeleted).Count()
                        let CompanyDriversCount = _context.Users.Where(x => x.TypeUser == UserType.Client.ToNumber() && x.AddedUserType == (int)AddedUserType.AddDeliverCompany && !x.IsDeleted).Count()
                        let DeliveryCompanyCount = _context.Users.Where(x => x.TypeUser == UserType.DeliverCompany.ToNumber() && !x.IsDeleted).Count()
                        let RentalCompanyCount = _context.Users.Where(x => x.TypeUser == UserType.RentalCompany.ToNumber() && !x.IsDeleted).Count()
                        let CityCount = _context.City.Where(x => !x.IsDeleted).Count()
                        let totalOrder = _context.Orders.Where(c => c.User.IsDeleted == false && c.RenewCompany.IsDeleted == false).Count()
                        let totalNewOrder = _context.Orders.Where(c => c.OrderStatus == (int)OrderStutes.NewOrder && !c.User.IsDeleted && !c.RenewCompany.IsDeleted).Count()
                        let totalCurrentOrder = _context.Orders.Where(c => c.OrderStatus == (int)OrderStutes.Current && !c.User.IsDeleted && !c.RenewCompany.IsDeleted).Count()
                        let totalCanceledOrder = _context.Orders.Where(c => c.OrderStatus == (int)OrderStutes.Refused && !c.User.IsDeleted && !c.RenewCompany.IsDeleted).Count()
                        select new DashBoardHomeModel
                        {
                            UserCount = UserCount,
                            CityCount = CityCount,
                            DeliveryCompanyCount = DeliveryCompanyCount,
                            DriverCount = DriverCount,
                            IndependentDriversCount = IndependentDriversCount,
                            CompanyDriversCount = CompanyDriversCount,
                            RentalCompanyCount = RentalCompanyCount,
                            totalCanceledOrder = totalCanceledOrder,
                            totalCurrentOrder = totalCurrentOrder,
                            totalNewOrder = totalNewOrder,
                            totalOrder = totalOrder
                        }).FirstOrDefault();

            return data;
        }
    }
}
