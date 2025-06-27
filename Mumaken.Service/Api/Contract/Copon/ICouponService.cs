using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Api.Contract.Copon
{
    public interface ICouponService
    {
        Task<bool> CheckExpiredCoupon(string couponCode);
        Task<bool> CheckCouponMaxUse(string couponCode);
        Task<bool> CheckCouponExist(string couponCode);
        Task<bool> CheckRentalCompany(string coponCode, string rentalCompanyId);
        Task<int> GetCoponId(string couponCode);
        Task<bool> CheckCouponUsage(string couponId, string userId);
        Task<double> GetLastTotalForUsingCoupon(string couponCode, double total);
        Task<double> GetTotalForUsingCoupon(string couponCode, double total);


    }
}
