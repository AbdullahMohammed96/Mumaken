using Mumaken.Domain.ViewModel.Notification;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Contract.NotificationInterfaces
{
    public interface INotificationServices
    {
        Task<List<HistoryNotificationViewModel>> GetHistoryNotify();
        Task<List<GetAllNotificationViewModel>> GetAllNotification();
        Task<List<UsersViewModel>> GetUsers();
        Task<List<UsersViewModel>> GetDeliveryCompany();
        Task<List<UsersViewModel>> GetRentalCompany();
        Task<bool> Send(string msg, string employees, string deliveryCompanyes,string rentalCompanyes);
        Task<bool> Notify(string textAr, string textEn, string fkProvider, int stutes, int orderId = 0);
    }
}
