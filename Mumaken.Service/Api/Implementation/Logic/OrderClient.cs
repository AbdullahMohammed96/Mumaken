using AAITHelper.Enums;
using AutoMapper.QueryableExtensions;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.OrderDTO;
using Mumaken.Domain.Enums;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Logic;
using Mumaken.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Common.Helpers.ResponseHelper;
using AAITHelper;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Service.Api.Contract.Copon;
using NPOI.SS.Formula.Functions;
using Mumaken.Domain.Common.Helpers.Extensions;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Entities.CarTable;
using Mumaken.Service.Api.Implementation.Auth;
using Mumaken.Domain.Entities.Copon;
using Mumaken.Domain.Entities.UserTables;
using AAITHelper.ModelHelper;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using NPOI.OpenXmlFormats.Spreadsheet;
namespace Mumaken.Service.Api.Implementation.Logic
{
    public class OrderClient : IOrderClient
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICouponService _coponServices;
        private readonly IHelper _helper;
        private readonly IHubContext<ChatHub> _hubContext;
        public OrderClient(ApplicationDbContext db, ICurrentUserService currentUserService, ICouponService coponServices, IHelper helper, IHubContext<ChatHub> hubContext)
        {
            _db = db;
            _currentUserService = currentUserService;
            _coponServices = coponServices;
            _helper = helper;
            _hubContext = hubContext;
        }
        public async Task<BaseResponseDto<object>> TestNotifcation(string userId)
        {
            var userContext = await _db.ConnectUser
                .Where(c => c.UserId == userId)
                .Select(c => c.ContextId)
                .ToListAsync();
            await _hubContext.Clients.Clients(userContext).SendAsync("AddNewOrderNotify", new { msg = "hello" });
            return ResponseHelper.Success<object>(true, "");
        }
        public async Task<BaseResponseDto<object>> AddNewOrder(OrderAddDto model)
        {
            var userId = _currentUserService.UserId;
            var currentUser = await _db.Users.FirstOrDefaultAsync(c => c.Id == userId);

            var addCar = await _db.Cars
                  .Where(c => c.Id == model.carInfo.carId)
                  .FirstOrDefaultAsync();

            if (addCar is null)
            {
                var carMessageError = model.lang == "ar" ? "لم يتم العثور علي السياره" : "Car Not Found";
                return ResponseHelper.Failure<object>(carMessageError);
            }
            if (!string.IsNullOrWhiteSpace(model.carInfo.CoponCode))
            {
                var (checkCoponResult, messageCheckCopon) = await ValidateCopon(model.carInfo.CoponCode,addCar.RentalCompanyId, model.lang);
                if (!checkCoponResult)
                    return ResponseHelper.Failure<object>(messageCheckCopon);
            }
            var orderCar = addCar.Adapt<OrderCar>();

            var newOrder = model.Adapt<Order>();

            newOrder.UserId = userId;
            newOrder.DrivingLicenseImage = _helper.Upload(model.userInfoDto.drivingLicenseImage, FileName.Users.ToNumber());
            newOrder.IdentityImage = _helper.Upload(model.userInfoDto.identityImage, FileName.Users.ToNumber());
            newOrder.OrderCar = orderCar;
            newOrder.RentalCompanyId = orderCar.RentalCompanyId;


            var totalPrice = addCar.RentalPriceDaily * model.carInfo.rentalPeriod;
            await CalcateOrderPrice(totalPrice, addCar.RentalPriceDaily, model.carInfo.rentalPeriod, newOrder, model.carInfo.CoponCode);
            if (model.carInfo.TypePay == (int)TypePay.walet)
            {
                if (newOrder.NetPrice > currentUser.Walet)
                {
                    return ResponseHelper.Failure<object>(HelperMsg.creatMessage(model.lang, "لا يوجد رصيد في المحفظه", "There is no balance in the wallet"));
                }
            }
            if (model.deliverCompanyInfo.workedCompany is not null)
            {
                foreach (var workedCompany in model.deliverCompanyInfo.workedCompany)
                {
                    newOrder.DeliverCompany.Add(new OrderDeliverCompany
                    {
                        DeliverCompanyId = workedCompany.deliverComapnyId,
                        UserDataInAppDelivery = workedCompany.loginData,
                        WorkWithDeliveryCompany = true,
                        TypeDriverInDeliveryCompanies = (int)TypeDriverInDeliveryCompanies.Update,
                        DeliverCompanyCase = (int)OrderCase.WaitAcceptAdministration

                    });
                }
            }

            if (model.deliverCompanyInfo.newComanyWantedWorkId is not null)
            {
                foreach (var wamtedWorkedCompany in model.deliverCompanyInfo.newComanyWantedWorkId)
                {
                    newOrder.DeliverCompany.Add(new OrderDeliverCompany
                    {
                        DeliverCompanyId = wamtedWorkedCompany,
                        WorkWithDeliveryCompany = false,
                        TypeDriverInDeliveryCompanies = (int)TypeDriverInDeliveryCompanies.Joining,
                        DeliverCompanyCase = (int)OrderCase.WaitAcceptAdministration

                    });
                }
            }

            try
            {
                await _db.Orders.AddAsync(newOrder);
                await _db.SaveChangesAsync();
                if (!string.IsNullOrWhiteSpace(model.carInfo.CoponCode))
                {
                    var coponUsedAdded = new CoponUsed
                    {
                        CoponId = model.carInfo.CoponCode,
                        OrderId = newOrder.Id,
                        UserId = userId
                    };
                    await _db.CoponUsed.AddAsync(coponUsedAdded);
                    await _db.SaveChangesAsync();
                }
                return ResponseHelper.Success<object>(newOrder.Id, model.lang == "ar" ? "تمت اضافه الطلب بنجاح" : "The Order Added successfully");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), model.lang));
            }

        }

        public async Task<BaseResponseDto<GetOrderPaymentInfoDto>> GetOrderPaymentInfo(int carId, string lang)
        {
            var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == carId);
            var (appPrice, appPercentage, vatPrice, vatPercentage) = await CalcateAppAndVatPrice(car.RentalPriceDaily);
            var textRenewAndCancel = await _db.Settings.Select(c => c.ChangeLangInformationRenewalOrcCancellation(lang)).FirstOrDefaultAsync();
            var orderInfo = new GetOrderPaymentInfoDto
            {
                discountPrice = 0,
                priceForDailyRental = car.RentalPriceDaily,
                varPrice = vatPrice,
                vatPercetage = vatPercentage,
                netPrice = car.RentalPriceDaily + vatPrice,
                subTotal = car.RentalPriceDaily + vatPrice,
                renewAndCancelText = textRenewAndCancel
            };
            return ResponseHelper.Success(orderInfo);
        }
        public async Task<BaseResponseDto<CoponPriceModelDto>> UseCopon(double priceForDailyRental, int period, string coponCode, int carId, string lang = "ar")
        {
            var car =await _db.Cars.Where(c => c.Id == carId).FirstOrDefaultAsync();
            var (resultValidateCopon, messageValidateCopon) = await ValidateCopon(coponCode, car.RentalCompanyId, lang);

            if (!resultValidateCopon)
            {
                var InvalidCoponsubTotalPrice = priceForDailyRental * period;
                var (InvalidCoponappPrice, InvalidCoponappPercentage, InvalidCoponvatPrice, InvalidCoponvatPercentage) = await CalcateAppAndVatPrice(InvalidCoponsubTotalPrice);
                var modelInvalidCopon = new CoponPriceModelDto
                {
                    priceForDailyRental = priceForDailyRental,
                    varPrice= InvalidCoponvatPrice,
                    vatPercetage= InvalidCoponvatPercentage,
                    netPrice= InvalidCoponsubTotalPrice+ InvalidCoponvatPrice,
                    subTotal= InvalidCoponsubTotalPrice+ InvalidCoponvatPrice
                };
                return ResponseHelper.Success<CoponPriceModelDto>(modelInvalidCopon, messageValidateCopon);
                //return ResponseHelper.Failure<CoponPriceModelDto>(messageValidateCopon);
            }
                

            var price = priceForDailyRental * period;
            var discountValue = await CalcateCoponPriceWithouthUpdateCountUsed(coponCode, priceForDailyRental);
            var subTotalPrice = discountValue * period;
            //var (appPrice, appPercentage, vatPrice, vatPercentage) = await CalcateAppAndVatPrice(price);
            var (appPrice, appPercentage, vatPrice, vatPercentage) = await CalcateAppAndVatPrice(subTotalPrice);
           

            var netPrice = (discountValue * period) + vatPrice;
            var subTotal = price + vatPrice;
            var model = new CoponPriceModelDto
            {
                priceForDailyRental = priceForDailyRental,
                netPrice = netPrice,
                subTotal = subTotal,
                varPrice = vatPrice,
                vatPercetage = vatPercentage,
               // discountPrice = subTotalPrice - netPrice
                discountPrice = subTotal - netPrice
            };
            return ResponseHelper.Success(model, HelperMsg.creatMessage(lang, "تم تفعيل الكوبون", "The coupon has been activated"));
        }
        public async Task<BaseResponseDto<Pagination<OrderListDto>>> GetOrderByStatus(GetOrderByStatusDto model)
        {
            var currentUserId = _currentUserService.UserId;
            var currentDate = HelperDate.GetCurrentDate();
            var cultureInfo = Helper.GetCulture(model.lang);
            var orders = await _db.Orders
                  .Include(c => c.RenewOrderdata)
                  .Where(c => c.UserId == currentUserId && c.OrderStatus == model.status)
                  .Select(c => new OrderListDto
                  {
                      orderId = c.Id,
                      orderCaseText = (c.HasRenewToAdditionalPeriod && c.RenewOrderdata != null && c.RenewOrderdata.Any())
                      ? Helper.OrderCaseText(c.RenewOrderdata.OrderByDescending(c => c.Id).Select(d => d.Case).FirstOrDefault(), model.lang)
                      : Helper.OrderCaseText(c.OrderCase, model.lang),

                      orderCase = c.OrderCase,

                      price = (c.HasRenewToAdditionalPeriod && c.RenewOrderdata != null && c.RenewOrderdata.Any())
                      ? c.RenewOrderdata.OrderByDescending(c => c.Id).Select(d => d.NetPrice + d.BreakPeriodPrice + d.RentalConfirmationDelayPrice).FirstOrDefault()
                      : c.NetPrice + c.BreakPeriodPrice + c.RentalConfirmationDelayPrice,

                      date = c.CreationDate.Humanize(true, currentDate, cultureInfo)

                  }).OrderByDescending(c => c.orderId).Paginate(model.pageNumber, model.pageSize);

            return ResponseHelper.Success(orders);
        }
        public async Task<BaseResponseDto<OrderDetailsDto>> GetOdrerDetails(int orderId, string lang = "ar")
        {
            var orderDetails = await GetOrderDetailsFromDatabase(orderId, lang);

            if (orderDetails == null)
                return ResponseHelper.Failure<OrderDetailsDto>(HelperMsg.creatMessage(lang, "الطلب غير موجود", "order Not Exsist"));


            return ResponseHelper.Success(orderDetails);
        }
        public async Task<BaseResponseDto<OrderDetailsDto>> GetOdrerDetailsForCompanies(int orderId, string lang = "ar")
        {
            var orderDetails = await GetOrderDetailsFromDatabaseForCompanies(orderId, lang);

            if (orderDetails == null)
                return ResponseHelper.Failure<OrderDetailsDto>(HelperMsg.creatMessage(lang, "الطلب غير موجود", "order Not Exsist"));

            return ResponseHelper.Success(orderDetails);
        }
        public async Task<BaseResponseDto<object>> CanceledOrder(int orderId, string reasonForCanceled, string lang = "ar")
        {
            var order = await _db.Orders.FirstOrDefaultAsync(c => c.Id == orderId);
            if (order is not null)
            {
                if (order.OrderStatus==(int)OrderStutes.Refused)
                    return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "تم رفض الطلب من قبل", "The Order was previously rejected"));
               
                
                order.OrderStatus = (int)OrderStutes.Canceled;
                order.OrderCase = (int)OrderCase.Canceled;
                order.ReasonToCancled = reasonForCanceled;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم الغاء الطلب بنجاح", "The Order Has been Canceled Successfully"));
            }
            return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "حدث خطاء ما", "Something went wrong "));
        }
        public async Task<BaseResponseDto<object>> GetOrderPriceForCancleContact(int orderId, string lang = "ar")
        {
            var order = await _db.Orders
                  .Include(c => c.RenewOrderdata)
                 .Include(c => c.OrderCar)
                 .FirstOrDefaultAsync(c => c.Id == orderId);

            var isRenew = order.HasRenewToAdditionalPeriod && order.RenewOrderdata != null && order.RenewOrderdata.Count() > 0 ? true : false;

            var priceForRentalCar = isRenew ? order.RenewOrderdata.FirstOrDefault(c => c.Status == (int)OrderStutes.Current).DiscountValueForDailyRental
                : order.DiscountValueForDailyRental != 0 ? order.DiscountValueForDailyRental : order.OrderCar.RentalPriceDaily;
            var countTakenCarDay = isRenew ? (HelperDate.GetCurrentDate() - order.RenewOrderdata.FirstOrDefault(c => c.Status == (int)OrderStutes.Current).DateRentalPeriod).Days
                                : (HelperDate.GetCurrentDate() - order.DateRentalPeriod.Date).Days;
            if (countTakenCarDay <= 0)
            {
                double price = 0;
                return ResponseHelper.Success<object>(price);
            }

            var priceForRentalCarDaily = priceForRentalCar * countTakenCarDay;
            var (appPrice, appPercentage, vatPrice, vatPercentage) = await CalcateAppAndVatPrice(priceForRentalCarDaily);
            var totalPrice = priceForRentalCarDaily + vatPrice;
            return ResponseHelper.Success<object>(totalPrice);
        }
        public async Task<BaseResponseDto<object>> CancelContarct(int orderId, string lang = "ar")
        {
            var order = await _db.Orders
                .Include(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Include(c => c.RenewCompany)
                .Include(c => c.RenewOrderdata)
                .Include(c => c.OrderCar)
                .FirstOrDefaultAsync(c => c.Id == orderId);

            order.IsCancelContract = true;

            order.OrderCase = (int)OrderCase.SendRequsetToCancelContract;

            order.DateCancelContact = HelperDate.GetCurrentDate();

            if (order.HasRenewToAdditionalPeriod)
            {
                var renewOrderdata = order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault();
                renewOrderdata.Status = (int)OrderStutes.Current;
                renewOrderdata.Case = (int)OrderCase.SendRequsetToCancelContract;
                renewOrderdata.CancelContractDate = HelperDate.GetCurrentDate();
                _db.Update(renewOrderdata);
            }

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            var notifyUser = new NotifyUser
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم إرسال طلب إلغاء التعاقد بنجاح علي الطلب رقم {order.Id}. نحن في انتظار تأكيد شركة التأجير{order.RenewCompany.user_Name} استلام السيارة حتى تتمكن من دفع قيمة الاستئجار.",
                TextEn = $"The cancellation request has been sent successfully For order Number{order.Id}. We are waiting for the rental company {order.RenewCompany.user_Name} to confirm the receipt of the car so you can pay the rental period.",
                Type = (int)NotifyTypes.CanacelContact,
                UserId = order.UserId
            };
            var notifyDelgate = new NotifyDelegt
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"قام العميل {order.User.user_Name} بإرسال طلب لإلغاء التعاقد علي الطلب رقم{order.Id}، يرجى تأكيد استلام السيارة حتى يتمكن العميل من دفع قيمة الاستئجار.",
                TextEn = $"The Client {order.User.user_Name} has sent a request to cancel the contract For Order Number{order.Id}. Please confirm receipt of the car so the client can pay the rental period.",
                Type = (int)NotifyTypes.CanacelContact,
                UserId = order.RentalCompanyId
            };

            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();

            await _db.NotifyUsers.AddAsync(notifyUser);
            await _db.NotifyDelegts.AddAsync(notifyDelgate);
            await _db.SaveChangesAsync();

          var contextIds=await  _db.ConnectUser.Where(c=>c.UserId==order.RentalCompanyId)
                .Select(c=>c.ContextId)
                .ToListAsync();

            if (contextIds.Any())
                await _hubContext.Clients.Clients(contextIds).SendAsync("AddNewOrderNotify", new { msg = notifyDelgate.TextAr });

            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                  , order.User.Lang == "ar" ? notifyUser.TextAr : notifyUser.TextEn, (int)NotifyTypes.CanacelContact, orderId, (int)OrderStutes.Current);

            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم الارسال بنجاح", "sent succesfully"));
        }
        public async Task<BaseResponseDto<object>> FinishContarct(int orderId, string lang = "ar")
        {
            var order = await _db.Orders
                            .Include(c => c.User)
                            .ThenInclude(c => c.DeviceId)
                            .Include(c => c.RenewCompany)
                            .Include(c => c.RenewOrderdata)
                            .Where(c => c.Id == orderId)
                            .FirstOrDefaultAsync();

            order.OrderCase = (int)OrderCase.SendRequsetToFinishedContract;
            order.DateCancelContact = HelperDate.GetCurrentDate();
            if (order.HasRenewToAdditionalPeriod)
            {
                var renewOrderdate = order.RenewOrderdata
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefault();

                renewOrderdate.Case = (int)OrderCase.SendRequsetToFinishedContract;
                renewOrderdate.CancelContractDate = HelperDate.GetCurrentDate();

                _db.Update(renewOrderdate);
            }
            _db.Update(order);

            await _db.SaveChangesAsync();
            var notifyUser = new NotifyUser
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم إرسال طلب إنهاء التعاقد بنجاح علي الطلب رقم {order.Id}. نحن في انتظار تأكيد شركة التأجير{order.RenewCompany.user_Name} استلام السيارة حتى تتمكن من دفع قيمة الاستئجار.",
                TextEn = $"The ending  request has been sent successfully For order Number{order.Id}. We are waiting for the rental company {order.RenewCompany.user_Name} to confirm the receipt of the car so you can pay the rental period.",
                Type = (int)NotifyTypes.CanacelContact,
                UserId = order.UserId
            };
            var notifyDelgate = new NotifyDelegt
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"قام العميل {order.User.user_Name} بإرسال طلب إنهاء التعاقد علي الطلب رقم{order.Id}، يرجى تأكيد استلام السيارة حتى يتمكن العميل من دفع قيمة الاستئجار.",
                TextEn = $"The Client {order.User.user_Name} has sent a request to end the contract For Order Number{order.Id}. Please confirm receipt of the car so the client can pay the rental period.",
                Type = (int)NotifyTypes.CanacelContact,
                UserId = order.RentalCompanyId
            };

            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();

            await _db.NotifyUsers.AddAsync(notifyUser);
            await _db.NotifyDelegts.AddAsync(notifyDelgate);
            await _db.SaveChangesAsync();

            var contextIds = await _db.ConnectUser.Where(c => c.UserId == order.RentalCompanyId)
                    .Select(c => c.ContextId)
                    .ToListAsync();

            if (contextIds.Any())
                await _hubContext.Clients.Clients(contextIds).SendAsync("AddNewOrderNotify", new { msg = notifyDelgate.TextAr });

            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                  , order.User.Lang == "ar" ? notifyUser.TextAr : notifyUser.TextEn, (int)NotifyTypes.CanacelContact, orderId, (int)OrderStutes.Current);
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم الارسال بنجاح", "sent succesfully"));
        }
        public async Task<BaseResponseDto<object>> ConfirmReciptCar(int orderId, string lang = "ar")
        {
            var order = await _db.Orders
                          .Include(c => c.User)
                          .ThenInclude(c => c.DeviceId)
                          .Include(c => c.RenewCompany)
                          .Include(c => c.RenewOrderdata)
                          .Where(c => c.Id == orderId)
                          .FirstOrDefaultAsync();



            bool isDailyFromRentalCompany = false;
            int dailyPeriodFromRentalCompany = 0;

            var currentdate = HelperDate.GetCurrentDate();
            order.IsConfirmReciptCar = true;
            order.OrderCase = (int)OrderCase.ConfirmReciptCar;
            order.DateReceiptCar = currentdate;
            if (order.IsCancelContract)   //  handel Recipt Car in Cancel Contract
            {
                int countTakenCarDays = 1;
                if (order.HasRenewToAdditionalPeriod)
                {
                    var renewOrder = order.RenewOrderdata
                        .OrderByDescending(c => c.Id)
                        .FirstOrDefault();
                    renewOrder.Case = (int)OrderCase.ConfirmReciptCar;
                    countTakenCarDays = (renewOrder.CancelContractDate.Date - renewOrder.DateRentalPeriod.Date).Days;

                    isDailyFromRentalCompany = currentdate.Date > renewOrder.CancelContractDate.Date;

                    dailyPeriodFromRentalCompany = isDailyFromRentalCompany ? (currentdate.Date - renewOrder.CancelContractDate.Date).Days : 0;
                    double priceForRentalCarDaily = GetPriceForRentalCarDaily(order, countTakenCarDays);
                    var subTotal = priceForRentalCarDaily * countTakenCarDays;

                    await CalcateOrderPrice(subTotal, priceForRentalCarDaily, countTakenCarDays, null, null, renewOrder);

                    if (isDailyFromRentalCompany)
                    {
                        renewOrder.RentalConfirmationDelayPeriod = dailyPeriodFromRentalCompany;
                        renewOrder.RentalConfirmationDelayPrice = dailyPeriodFromRentalCompany * priceForRentalCarDaily;
                    }
                    _db.Update(renewOrder);
                }
                else
                {
                    countTakenCarDays = (order.DateCancelContact - order.DateRentalPeriod.Date).Days;
                    if (countTakenCarDays == 0)
                        countTakenCarDays = 1;


                    isDailyFromRentalCompany = currentdate.Date > order.DateCancelContact.Date;

                    dailyPeriodFromRentalCompany = isDailyFromRentalCompany ? (currentdate.Date - order.DateCancelContact.Date).Days : 0;


                    double priceForRentalCarDaily = GetPriceForRentalCarDaily(order, countTakenCarDays);
                    var subTotal = priceForRentalCarDaily * countTakenCarDays;
                    await CalcateOrderPrice(subTotal, priceForRentalCarDaily, countTakenCarDays, order, null);
                    if (isDailyFromRentalCompany)
                    {
                        order.RentalConfirmationDelayPeriod = dailyPeriodFromRentalCompany;
                        order.RentalConfirmationDelayPrice = dailyPeriodFromRentalCompany * priceForRentalCarDaily;
                    }

                }

                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
            }
            else   // finish Order
            {
                bool isBreakPeriod = false;
                int breakPeriod = 0;
                if (order.HasRenewToAdditionalPeriod)
                {
                    var renewOrderData = order.RenewOrderdata
                        .OrderByDescending(c => c.Id)
                        .FirstOrDefault();
                    renewOrderData.Case = (int)OrderCase.ConfirmReciptCar;
                    //isBreakPeriod = currentdate.Date > renewOrderData.DateEndRentalPeriod.Date;
                    isBreakPeriod = renewOrderData.CancelContractDate.Date > renewOrderData.DateEndRentalPeriod.Date;
                    if (isBreakPeriod)
                    {
                        //breakPeriod = (currentdate.Date - renewOrderData.DateEndRentalPeriod.Date).Days;
                        breakPeriod = (renewOrderData.CancelContractDate.Date - renewOrderData.DateEndRentalPeriod.Date).Days;
                        renewOrderData.BreakPeriod = breakPeriod;

                        renewOrderData.BreakPeriodPrice = GetPriceForRentalCarDaily(order, breakPeriod) * breakPeriod;

                    }
                    isDailyFromRentalCompany = currentdate.Date > renewOrderData.CancelContractDate.Date;
                    if (isDailyFromRentalCompany)
                    {
                        dailyPeriodFromRentalCompany = (currentdate.Date - renewOrderData.CancelContractDate.Date).Days;
                        renewOrderData.RentalConfirmationDelayPeriod = dailyPeriodFromRentalCompany;
                        renewOrderData.RentalConfirmationDelayPrice = GetPriceForRentalCarDaily(order, dailyPeriodFromRentalCompany) * dailyPeriodFromRentalCompany;

                    }
                    _db.Update(renewOrderData);
                }
                else
                {
                    //isBreakPeriod = currentdate.Date > order.DateEndRentalPeriod.Date;
                    isBreakPeriod = order.DateCancelContact.Date > order.DateEndRentalPeriod.Date;
                    if (isBreakPeriod)
                    {
                        breakPeriod = (order.DateCancelContact.Date - order.DateEndRentalPeriod.Date).Days;
                        order.BreakPeriod = breakPeriod;

                        order.BreakPeriodPrice = GetPriceForRentalCarDaily(order, breakPeriod) * breakPeriod;
                    }
                    isDailyFromRentalCompany = currentdate.Date > order.DateCancelContact.Date;
                    if (isDailyFromRentalCompany)
                    {
                        dailyPeriodFromRentalCompany = (currentdate.Date - order.DateCancelContact.Date).Days;
                        order.RentalConfirmationDelayPeriod = dailyPeriodFromRentalCompany;
                        order.RentalConfirmationDelayPrice = GetPriceForRentalCarDaily(order, dailyPeriodFromRentalCompany) * dailyPeriodFromRentalCompany;
                    }
                }
                _db.Update(order);
                await _db.SaveChangesAsync();
            }
            var notifyUser = new NotifyUser
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم تأكيد استلام السيارة من قبل شركة التأجير{order.RenewCompany.user_Name}. برجاء دفع قيمة الطلب رقم {order.Id}.",
                TextEn = $"The receipt of the car has been confirmed by the rental company{order.RenewCompany.user_Name}. Please pay the amount for order number {order.Id}.",
                Type = (int)NotifyTypes.ConfirmReciptCar,
                UserId = order.UserId
            };

            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();

            await _db.NotifyUsers.AddAsync(notifyUser);

            await _db.SaveChangesAsync();

            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                  , order.User.Lang == "ar" ? notifyUser.TextAr : notifyUser.TextEn, (int)NotifyTypes.ConfirmReciptCar, orderId, (int)OrderStutes.Current);

            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم استلام السيارة بنجاح", "The car was received successfully"));
        }
        public async Task<BaseResponseDto<CoponPriceModelDto>> UseCoponInOrder(int orderId, string coponCode, string lang = "ar")
        {
         
            var order = await _db.Orders
                .Include(c => c.RenewOrderdata)
                .Include(c => c.OrderCar)
                .FirstOrDefaultAsync(c => c.Id == orderId);


            var isUseCoponInOrder = await _db.CoponUsed
                 .Where(c => c.OrderId == orderId).AnyAsync();
            if (isUseCoponInOrder)
                return ResponseHelper.Failure<CoponPriceModelDto>(HelperMsg.creatMessage(lang, "تم استخدام كوبون خصم في الطلب من قبل", "A discount coupon was used in the order before"));


            var isUserUserCopon = await _db.CoponUsed
                 .Where(c => c.UserId == order.UserId && c.CoponId == coponCode).AnyAsync();
            if (isUserUserCopon)
                return ResponseHelper.Failure<CoponPriceModelDto>(HelperMsg.creatMessage(lang, "تم استخدام كوبون خصم من قبل", "A discount coupon was used before"));

            var (resultValidateCopon, messageValidateCopon) = await ValidateCopon(coponCode, order.RentalCompanyId, lang);

            if (!resultValidateCopon)
                return ResponseHelper.Failure<CoponPriceModelDto>(messageValidateCopon);
            if (order.CoponCode != null && order.CoponCode.ToLower() == coponCode.ToLower())
            {
                return ResponseHelper.Failure<CoponPriceModelDto>(HelperMsg.creatMessage(lang, "تم استخدام الكوبون من قبل", "The coupon has been used before"));
            }
            //var priceWithoutVat = order.OrderCar.RentalPriceDaily * order.RentalPeriod;
            if (order.HasRenewToAdditionalPeriod)
            {
                
                var reneworderData = order.RenewOrderdata
                 .OrderByDescending(c=>c.Id)
                 .FirstOrDefault();
                var priceWithoutVat = order.OrderCar.RentalPriceDaily * reneworderData.Duration;
                await CalcateOrderPrice(priceWithoutVat, order.OrderCar.RentalPriceDaily, reneworderData.Duration, null, coponCode, reneworderData);
                _db.RenewOrderdata.Update(reneworderData);
            }
            else
            {
                var priceWithoutVat = order.OrderCar.RentalPriceDaily * order.RentalPeriod;
                await CalcateOrderPrice(priceWithoutVat, order.OrderCar.RentalPriceDaily, order.RentalPeriod, order, coponCode);
            }


            if (!string.IsNullOrWhiteSpace(coponCode))
            {
                var coponUsedAdded = new CoponUsed
                {
                    CoponId = coponCode,
                    OrderId = order.Id,
                    UserId = order.UserId
                };
                await _db.CoponUsed.AddAsync(coponUsedAdded);
            }
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            var renewOrderdata = order.HasRenewToAdditionalPeriod
                        ? order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault()
                        : null;
            
            var model = new CoponPriceModelDto
            {
                priceForDailyRental = order.OrderCar.RentalPriceDaily,
                //netPrice = netPriceAfetrAddCopon,
                //discountPrice = netPriceBeforeAddCopon - netPriceAfetrAddCopon
                netPrice =  renewOrderdata != null ? renewOrderdata.NetPrice + renewOrderdata.RentalConfirmationDelayPeriod + renewOrderdata.BreakPeriodPrice 
                        : order.NetPrice+ order.BreakPeriodPrice+ order.RentalConfirmationDelayPrice,
                discountPrice = renewOrderdata != null? renewOrderdata.coponPrice.Value: order.coponPrice.Value,
                subTotal = renewOrderdata != null ? renewOrderdata.subTotal : order.subTotal,
                varPrice = renewOrderdata != null ? renewOrderdata.VatPrice: order.VatPrice,
                vatPercetage = renewOrderdata != null ? renewOrderdata.VatPercetage : order.VatPercetage,

                breakPeriod = renewOrderdata != null ? renewOrderdata.BreakPeriod : order.BreakPeriod,
                breakPrice = renewOrderdata != null ? renewOrderdata.BreakPeriodPrice : order.BreakPeriodPrice,

                dailyRentalCompanyPeriod = renewOrderdata != null ? renewOrderdata.RentalConfirmationDelayPeriod : order.RentalConfirmationDelayPeriod,
                dailyRentalCompanyPrice= renewOrderdata != null ? renewOrderdata.RentalConfirmationDelayPrice : order.RentalConfirmationDelayPrice,
            };
            return ResponseHelper.Success(model, HelperMsg.creatMessage(lang, "تم تفعيل الكوبون بنجاح", "The coupon has been activated successfully"));
        }
        public async Task<BaseResponseDto<object>> PayRentalPeriod(int orderId, int typePay, string lang = "ar")
        {
            var order = await _db.Orders
                    .Include(c => c.User)
                    .Include(c => c.RenewOrderdata)
                    .Where(c => c.Id == orderId)
                    .FirstOrDefaultAsync();

            double price = 0;
            if (typePay == (int)TypePay.walet)
            {
                if (order.HasRenewToAdditionalPeriod == true && order.RenewOrderdata.Count() > 0)
                {
                    var renewOrder = order.RenewOrderdata
                    .OrderByDescending(c => c.Id).FirstOrDefault();

                    price = renewOrder.NetPrice + renewOrder.BreakPeriodPrice;
                }
                price = order.NetPrice + order.BreakPeriodPrice;

                if (price > order.User.Walet)
                {
                    return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "لا يوجد رصيد في المحفظه", "There is no balance in the wallet"));
                }
            }

            order.OrderStatus = (int)OrderStutes.Current;
            order.OrderCase = (int)OrderCase.PaymentOrderAndWaitConfirmRentanlCompany;
            order.IsPaid = true;

            FinancialAccount financialAccount = new FinancialAccount();

            if (order.HasRenewToAdditionalPeriod)
            {
                var renewOrderData = order.RenewOrderdata
                        .OrderByDescending(c => c.Id)
                        .FirstOrDefault();

                renewOrderData.Status = (int)OrderStutes.Finished;
                renewOrderData.Case = (int)OrderCase.PaymentOrderAndWaitConfirmRentanlCompany;
                renewOrderData.IsPaid = true;
                financialAccount = CreateFinancialAccount(order, typePay, renewOrderData);

                _db.Update(renewOrderData);
            }
            else
            {
                financialAccount = CreateFinancialAccount(order, typePay, null);
            }
            if (typePay == (int)TypePay.walet)
            {
                order.User.Walet -= price;
            }
            await FinishOrderInDeliveryCompany(orderId);
            await CreateReport(order.Id, order.RentalCompanyId);
            await _db.SaveChangesAsync();
            var notfiyDelegt = new NotifyDelegt
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"قام العميل {order.User.user_Name}بدفع الطلب رقم {order.Id}. برجاء تأكيد استلام المبلغ.",
                TextEn = $"The client  {order.User.user_Name}has paid for order number {order.Id}. Please confirm receipt of the amount.",
                Type = (int)NotifyTypes.ConfirmReceiptAmount,
                UserId = order.RentalCompanyId
            };
            await _db.NotifyDelegts.AddAsync(notfiyDelegt);
            await _db.SaveChangesAsync();

            var contextIds = await _db.ConnectUser.Where(c => c.UserId == order.RentalCompanyId)
                    .Select(c => c.ContextId)
                    .ToListAsync();

            if (contextIds.Any())
                await _hubContext.Clients.Clients(contextIds).SendAsync("AddNewOrderNotify", new { msg = notfiyDelegt.TextAr });

            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم دفع قيمه الاستئجار بنجاح", "The rental price has been paid successfully"));

        }
        public async Task<BaseResponseDto<object>> RenewOrderToAnotherPeriod(int orderId, int typePay, int rentalPeriod, string lang)
        {
            if (rentalPeriod <= 0)
            {
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "ادخل مده الاستئجار بطريقه صحيحه", "Enter the rental period correctly"));
            }

            var order = await GetOrderWithDetailsAsync(orderId);
            double price = 0;

            if (typePay == (int)TypePay.walet)
            {
                if (order.HasRenewToAdditionalPeriod == true && order.RenewOrderdata.Count() > 0)
                {
                    var renewOrder = order.RenewOrderdata
                    .OrderByDescending(c => c.Id).FirstOrDefault();

                    price = renewOrder.NetPrice + renewOrder.BreakPeriodPrice;
                }
                price = order.NetPrice + order.BreakPeriodPrice;

                if (price > order.User.Walet)
                {
                    return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "لا يوجد رصيد في المحفظه", "There is no balance in the wallet"));
                }
            }

            await HandleRenewOrder(order, rentalPeriod, typePay);

            if (typePay == (int)TypePay.walet)
            {
                order.User.Walet -= price;
            }
            await CreateReport(order.Id, order.RentalCompanyId);
            await _db.SaveChangesAsync();

            var notifyUser = new NotifyUser
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم تجديد فترة الطلب رقم {order.Id} لمدة جديدة.",
                TextEn = $"The rental period for order number {order.Id} has been renewed for a new duration.",
                Type = (int)NotifyTypes.CanacelContact,
                UserId = order.UserId
            };
            var notifyDelgate = new NotifyDelegt
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم تجديد فترة الطلب رقم {order.Id} لمدة جديدة من قبل العميل {order.User.user_Name}.",
                TextEn = $"The rental period for order number {order.Id} has been renewed for a new duration by the client {order.User.user_Name}.",
                Type = (int)NotifyTypes.CanacelContact,
                UserId = order.RentalCompanyId
            };
            await _db.NotifyUsers.AddAsync(notifyUser);
            await _db.NotifyDelegts.AddAsync(notifyDelgate);
            await _db.SaveChangesAsync();
            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                  , order.User.Lang == "ar" ? notifyUser.TextAr : notifyUser.TextEn, (int)NotifyTypes.RenewOrder, orderId, (int)OrderStutes.Current);


            var contextIds = await _db.ConnectUser.Where(c => c.UserId == order.RentalCompanyId)
                    .Select(c => c.ContextId)
                    .ToListAsync();

            if (contextIds.Any())
                await _hubContext.Clients.Clients(contextIds).SendAsync("AddNewOrderNotify", new { msg = notifyDelgate.TextAr });

            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم تجديد الطلب لفترة أخرى بنجاح", "Order has been successfully renewed for another period."));
        }
        public async Task<BaseResponseDto<object>> PaymentOrder(int orderId, int typeOrderPayment, int typePay, int rentalPeriod, string lang)
        {
            if (rentalPeriod == (int)PaymentOrderType.PaymentAndRenewPeriod)
            {
                if (rentalPeriod <= 0)
                {
                    return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "ادخل مده الاستئجار بطريقه صحيحه", "Enter the rental period correctly"));
                }
            }
            var order = await GetOrderWithDetailsAsync(orderId);
            double price = 0;

            if (order == null)
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "الطلب غير موجود", "Order not found"));

            if (typePay == (int)TypePay.walet)
            {
                price = order.HasRenewToAdditionalPeriod == true && order.RenewOrderdata.Count() > 0 ? order.RenewOrderdata
                    .OrderByDescending(c => c.Id).FirstOrDefault().NetPrice : order.NetPrice;

                if (price > order.User.Walet)
                {
                    return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "لا يوجد رصيد في المحفظه", "There is no balance in the wallet"));
                }
            }

            switch (typeOrderPayment)
            {
                case (int)PaymentOrderType.PaymentOrderToEndRentalPeriod:
                    await HandleEndRentalPeriodPayment(order, typePay);
                    break;
                case (int)PaymentOrderType.CancelContract:
                    await HandleCancelContract(order, typePay);
                    break;
                default:
                    await HandleRenewOrder(order, rentalPeriod, typePay);
                    break;
            }
            if (typePay == (int)TypePay.walet)
            {
                order.User.Walet -= price;
            }
            if (typeOrderPayment == (int)PaymentOrderType.PaymentOrderToEndRentalPeriod || typeOrderPayment == (int)PaymentOrderType.CancelContract)
            {
                await FinishOrderInDeliveryCompany(orderId);

            }

            await CreateReport(order.Id, order.RentalCompanyId);
            await _db.SaveChangesAsync();


            return ResponseHelper.Success<object>(true, Helper.paymentOrderText(typeOrderPayment, lang));
        }
        public async Task<BaseResponseDto<object>> AddDeliveryCompaniesToCurentOrder(AddDeliveryCompanyToCurrentOrderDto model)
        {
            var orderWorkedCompanyAdd = new List<OrderDeliverCompany>();
            var orderNotWorkedCompanyAdd = new List<OrderDeliverCompany>();
            if (model.workedCompany is not null)
            {

                foreach (var item in model.workedCompany)
                {
                    var checkIfHasDeliveryCompany = await _db.OrderDeliverCompanies
                         .Where(c => c.DeliverCompanyId == item.deliverCompanyId && c.OrderId == model.OrderId)
                         .AnyAsync();
                    if (checkIfHasDeliveryCompany)
                    {
                        return ResponseHelper.Failure<object>(HelperMsg.creatMessage(model.lang, "الشركه موجوده بالفعل الرجاء اختيار شركه توصيل اخرى", "The company already exists. Please choose another delivery company"));
                    }
                    orderWorkedCompanyAdd.Add(new OrderDeliverCompany
                    {
                        OrderId = model.OrderId,
                        DeliverCompanyId = item.deliverCompanyId,
                        DeliverCompanyCase = (int)OrderCase.WaitToAcceptDeliverCompany,
                        WorkWithDeliveryCompany = true,
                        TypeDriverInDeliveryCompanies = (int)TypeDriverInDeliveryCompanies.Update,
                        UserDataInAppDelivery = item.loginData,
                    });
                }
                await _db.OrderDeliverCompanies.AddRangeAsync(orderWorkedCompanyAdd);
            }
            if (model.newCompanyWantedWorkId is not null)
            {
                foreach (var item in model.newCompanyWantedWorkId)
                {

                    var checkIfHasDeliveryCompany = await _db.OrderDeliverCompanies
                           .Where(c => c.DeliverCompanyId == item && c.OrderId == model.OrderId)
                           .AnyAsync();
                    if (checkIfHasDeliveryCompany)
                    {
                        return ResponseHelper.Failure<object>(HelperMsg.creatMessage(model.lang, "الشركه موجوده بالفعل الرجاء اختيار شركه توصيل اخرى", "The company already exists. Please choose another delivery company"));
                    }
                    orderNotWorkedCompanyAdd.Add(new OrderDeliverCompany
                    {
                        OrderId = model.OrderId,
                        DeliverCompanyId = item,
                        DeliverCompanyCase = (int)OrderCase.WaitToAcceptDeliverCompany,
                        WorkWithDeliveryCompany = false,
                        TypeDriverInDeliveryCompanies = (int)TypeDriverInDeliveryCompanies.Joining,
                    });
                }
                await _db.OrderDeliverCompanies.AddRangeAsync(orderNotWorkedCompanyAdd);
            }


            await _db.SaveChangesAsync();



            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تمت الاضافة بنجاح", "Added successfully"));
        }
        #region Helper

        public async Task<(double appPrice, double appPercentage, double vatPrice, double vatPercentage)> CalcateAppAndVatPrice(double price)
        {
            var setting = await _db.Settings.FirstOrDefaultAsync();
            var appPrice = (setting.AppPercentage * price) / 100;
            var vatPrice = (setting.VatPercetage * price) / 100;
            return (appPrice, setting.AppPercentage, vatPrice, setting.VatPercetage);
        }
        public async Task<(bool, string)> ValidateCopon(string coponCode,string rentalCompanyId, string lang = "ar")
        {
            var userId = _currentUserService.UserId;
            var checkCouponExist = await _coponServices.CheckCouponExist(coponCode);

            if (!checkCouponExist)
                return (false, HelperMsg.creatMessage(lang, "الكوبون المدخل غير موجود", "The Enter Copon Not Founded"));

            var checkRentalCompany = await _coponServices.CheckRentalCompany(coponCode, rentalCompanyId);

            if (!checkRentalCompany)
                return (false, HelperMsg.creatMessage(lang, "لا يمكنك استخدام هذا الكوبون مع هذه الشركة", "Sorry, this coupon is not valid for the selected rental company."));

            var checkCouponMaxUse = await _coponServices.CheckCouponMaxUse(coponCode);

            if (checkCouponMaxUse)
                return (false, HelperMsg.creatMessage(lang, "تم الوصول للحد الاقصي من استخدام الكوبون", "The maximum usage of the coupon has been reached"));

            var checkUsedCoponForUser = await _coponServices.CheckCouponUsage(coponCode, userId);
            if (checkUsedCoponForUser)
                return (false, HelperMsg.creatMessage(lang, "تم استخدام الكوبون من قبل ", "The coupon has been used before"));
            var checkExpiredCoupon = await _coponServices.CheckExpiredCoupon(coponCode);

            if (checkExpiredCoupon)
                return (false, HelperMsg.creatMessage(lang, "عذرا لقد انتهت مده صلاحيه الكوبون", "Sorry, the validity of the coupon has expired"));
           
           

            return (true, "");
        }
        public async Task<double> CalcateCoponPriceWithouthUpdateCountUsed(string coponCode, double subtotalPrice)
        {
            var netPrice = await _coponServices.GetLastTotalForUsingCoupon(coponCode, subtotalPrice);
            return netPrice;

        }
        public async Task<double> CalcateCoponPriceWithUpdateCountUsed(string coponCode, double subtotalPrice)
        {
            var netPrice = await _coponServices.GetTotalForUsingCoupon(coponCode, subtotalPrice);
            return netPrice;

        }
        private async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _db.Orders
                .Include(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Include(c => c.RenewCompany)
                .Include(c => c.OrderCar)
                .Include(c => c.RenewOrderdata)
                .FirstOrDefaultAsync(c => c.Id == orderId);
        }
        private async Task HandleEndRentalPeriodPayment(Order order, int typePay)
        {
            order.IsPaid = true;
            order.OrderStatus = (int)OrderStutes.Finished;
            order.OrderCase = (int)OrderCase.Finished;
            FinancialAccount financialAccount = new FinancialAccount();
            if (order.HasRenewToAdditionalPeriod)
            {
                var renewOrder = order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault();
                if (renewOrder != null)
                {
                    renewOrder.Status = (int)OrderStutes.Finished;
                    renewOrder.Case = (int)OrderCase.Finished;
                    financialAccount = CreateFinancialAccount(order, typePay, renewOrder);
                }
            }
            else
            {
                financialAccount = CreateFinancialAccount(order, typePay);
            }


            await _db.FinancialAccounts.AddAsync(financialAccount);
            _db.Orders.Update(order);
        }
        private async Task HandleCancelContract(Order order, int typePay)
        {
            order.OrderStatus = (int)OrderStutes.Finished;
            order.OrderCase = (int)OrderCase.CancelContractAndPaidRentalPeriod;

            int countTakenCarDays = 0;
            if (order.HasRenewToAdditionalPeriod)
            {
                var renewOrder = order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault();
                if (renewOrder != null)
                {
                    renewOrder.Status = (int)OrderStutes.Finished;
                    renewOrder.Case = (int)OrderCase.CancelContractAndPaidRentalPeriod;
                    renewOrder.CancelContractDate = HelperDate.GetCurrentDate();
                    renewOrder.TypePay = typePay;
                    countTakenCarDays = (DateTime.Now - renewOrder.DateRentalPeriod.Date).Days;

                    _db.Update(renewOrder);
                }
            }
            else
            {
                order.TypePay = typePay;
                order.DateCancelContact = HelperDate.GetCurrentDate();
                countTakenCarDays = (HelperDate.GetCurrentDate() - order.DateRentalPeriod.Date).Days;
            }


            double priceForRentalCarDaily = GetPriceForRentalCarDaily(order, countTakenCarDays);
            var subTotal = priceForRentalCarDaily * countTakenCarDays;
            FinancialAccount financialAccount = new FinancialAccount();
            if (order.HasRenewToAdditionalPeriod)
            {
                var renewOrder = order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault();
                await CalcateOrderPrice(subTotal, priceForRentalCarDaily, countTakenCarDays, null, null, renewOrder);

            }
            else
            {
                await CalcateOrderPrice(subTotal, priceForRentalCarDaily, countTakenCarDays, order, null);
            }

            financialAccount = CreateFinancialAccount(order, typePay);

            await _db.FinancialAccounts.AddAsync(financialAccount);



            _db.Orders.Update(order);
        }
        private async Task HandleRenewOrder(Order order, int rentalPeriod, int typePay)
        {
            var oldRenewOrder = order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault();
            var currentdate = HelperDate.GetCurrentDate();
            if (order.RenewOrderdata != null && order.RenewOrderdata.Any())
            {
                if (oldRenewOrder != null)
                {
                    oldRenewOrder.IsPaid = true;
                    oldRenewOrder.Status = (int)OrderStutes.Finished;
                    oldRenewOrder.Case = (int)OrderCase.Finished;
                }
            }

            var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == order.OrderCar.CarId);
            if (car == null) return;

            double totalPrice = car.RentalPriceDaily * rentalPeriod;
            var renewOrderData = new RenewOrderdata
            {
                OrderId = order.Id,
                DateRentalPeriod = DateTime.UtcNow.AddHours(3),
                DateEndRentalPeriod = currentdate.AddDays(rentalPeriod),
                Duration = rentalPeriod,
                Case = (int)OrderCase.RenewRentalPeriod,
                Status = (int)OrderStutes.Current,
            };

            await CalcateOrderPrice(totalPrice, car.RentalPriceDaily, rentalPeriod, null, null, renewOrderData);

            var financialAccount = order.HasRenewToAdditionalPeriod ? CreateFinancialAccount(order, typePay, oldRenewOrder)
                                                 : CreateFinancialAccount(order, typePay);


            order.OrderStatus = (int)OrderStutes.Current;
            order.OrderCase = (int)OrderCase.RenewRentalPeriod;
            order.HasRenewToAdditionalPeriod = true;

            order.RenewOrderdata.Add(renewOrderData);

            await _db.FinancialAccounts.AddAsync(financialAccount);
            _db.Orders.Update(order);
         
        }
        private double GetPriceForRentalCarDaily(Order order, int countTakenCarDays)
        {
            double priceForRentalCar = order.HasRenewToAdditionalPeriod
                ? order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault()?.DiscountValueForDailyRental ?? order.OrderCar.RentalPriceDaily
                : order.DiscountValueForDailyRental != 0 ? order.DiscountValueForDailyRental : order.OrderCar.RentalPriceDaily;

            // return priceForRentalCar * countTakenCarDays;
            return priceForRentalCar;
        }

        private async Task CreateReport(int orderId, string rentalCompanyId)
        {
            var report = new Report { OrderId = orderId, RentalCompayId = rentalCompanyId };
            await _db.Reports.AddAsync(report);
        }

        private async Task FinishOrderInDeliveryCompany(int orderId)
        {
            var acceptedDeliveryCompanies = await _db.OrderDeliverCompanies
                .Where(c => c.OrderId == orderId && c.DeliverCompanyCase == (int)OrderCase.Accepted)
                .ToListAsync();

            foreach (var deliveryCompany in acceptedDeliveryCompanies)
            {
                deliveryCompany.DeliverCompanyCase = (int)OrderCase.Finished;
            }

            _db.UpdateRange(acceptedDeliveryCompanies);
        }
        public FinancialAccount CreateFinancialAccount(Order order, int typePay, RenewOrderdata renewOrderdata = null)
        {
            return new FinancialAccount
            {
                Confirm = false,
                OrderId = order.Id,
                TotalOrderPrice = renewOrderdata != null ? renewOrderdata.NetPrice : order.NetPrice,
                ProviderPrice = typePay == (int)TypePay.cash ? 0 : renewOrderdata != null ? renewOrderdata.NetPrice - renewOrderdata.AppPrice
                : order.NetPrice - order.AppPrice,
                AppTax = renewOrderdata != null ? renewOrderdata.AppPrice : order.AppPrice,
                TypeOrderOrderPay = typePay == (int)TypePay.cash ? (int)PaymentMethod.Cash : (int)PaymentMethod.Online,
                RentalCompanyId = order.RentalCompanyId
            };
        }
        public async Task CalcateOrderPrice(double totalPriceWithoutDiscount, double dailyPriceForCar, int rentalPeriod, Order addOrder = null, string coponCode = "", RenewOrderdata renewOrderdata = null)
        {
            double discountValueForCarDailyRental = dailyPriceForCar;

            double totalPriceWithoutVat = totalPriceWithoutDiscount;
            if (!string.IsNullOrWhiteSpace(coponCode))
            {
                // Apply copon on Rental Car
                // take CoponPrice From Daily Price For Rental Car (سعر التاجير لليوم الواحد للسيارة)
                discountValueForCarDailyRental = await CalcateCoponPriceWithUpdateCountUsed(coponCode, dailyPriceForCar); 
                totalPriceWithoutVat = discountValueForCarDailyRental * rentalPeriod;
            }
            // Calcate VatPrice  After  Apply Copn Price Ex = 30*72=2700  // 72 is price After Apply Copn
            var (appPrice, appPercentage, vatPrice, vatPercentage) = await CalcateAppAndVatPrice(totalPriceWithoutVat);
            
            // Calcate VatPrice before Apply Copn Price Ex = 30*90=2700
          //  var(appPriceWithoutDiscount, appPercentageWithoutDiscount, vatPriceWithoutDiscount, vatPercentageWithoutDiscount) = await CalcateAppAndVatPrice(totalPriceWithoutDiscount);
            //كان هنا غلط في الحسابات المفروض بطبق الضريبه بعد الخصم عملن زي الكارت او السله 
            //اللي كان موجود اني بضيف الضريبه قبل الخصم ع التوتال فكان بيحسب غلط في الخصم 
            // لو انت فاضي واشتغل المشروع دا ممكن تعمل ريفاكتور شويه للكود 
            var subtotalPrice = totalPriceWithoutDiscount + vatPrice;  // السعر +الضريبه
            double netPrice = totalPriceWithoutVat + vatPrice;
            double discountValue = subtotalPrice - netPrice;
            var dailyRentalDiscount = dailyPriceForCar - discountValueForCarDailyRental;

            var discountValueForDailyRental = (dailyRentalDiscount) == 0 ? dailyPriceForCar : dailyRentalDiscount;

            if (addOrder != null)
            {
                addOrder.AppPrice = appPrice;
                addOrder.AppPercetage = appPercentage;
                addOrder.VatPrice = vatPrice;
                addOrder.VatPercetage = vatPercentage;
                addOrder.coponPrice = discountValue;
                addOrder.subTotal = subtotalPrice;
                addOrder.NetPrice = netPrice;
                addOrder.DiscountValueForDailyRental = discountValueForDailyRental;

                addOrder.BreakPeriodPrice = discountValueForDailyRental * addOrder.BreakPeriod;
                addOrder.RentalConfirmationDelayPrice = discountValueForDailyRental * addOrder.RentalConfirmationDelayPrice;
            }

            if (renewOrderdata != null)
            {
                renewOrderdata.AppPrice = appPrice;
                renewOrderdata.AppPercetage = appPercentage;
                renewOrderdata.VatPrice = vatPrice;
                renewOrderdata.VatPercetage = vatPercentage;
                renewOrderdata.coponPrice = discountValue;
                renewOrderdata.subTotal = subtotalPrice;
                renewOrderdata.NetPrice = netPrice;
                renewOrderdata.DiscountValueForDailyRental = discountValueForDailyRental;

                renewOrderdata.BreakPeriodPrice = discountValueForDailyRental * renewOrderdata.BreakPeriod;
                renewOrderdata.RentalConfirmationDelayPrice = discountValueForDailyRental * renewOrderdata.RentalConfirmationDelayPrice;
            }

        }
        #endregion
        #region  OrderDetails
        private async Task<OrderDetailsDto> GetOrderDetailsFromDatabase(int orderId, string lang = "ar")
        {
            var currentDate = HelperDate.GetCurrentDate();
            var cultureInfo = Helper.GetCulture(lang);

            var order = await _db.Orders
                .Include(c => c.OrderCar)
                .Include(c => c.RenewOrderdata)
                .Include(c => c.DeliverCompany)
                .ThenInclude(c => c.DeliverCompany)
                .Include(c => c.Nationality)
                .Include(c => c.User)
                .ThenInclude(c => c.city)
                .Include(c => c.RenewCompany)
                .Where(c => c.Id == orderId)
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            return MapOrderToOrderDetailsDto(order, currentDate, cultureInfo, lang);
        }
        private async Task<OrderDetailsDto> GetOrderDetailsFromDatabaseForCompanies(int orderId, string lang = "ar")
        {
            var currentDate = HelperDate.GetCurrentDate();
            var cultureInfo = Helper.GetCulture(lang);

            var order = await _db.Orders
                .Include(c => c.OrderCar)
                .Include(c => c.RenewOrderdata)
                .Include(c => c.DeliverCompany)
                .ThenInclude(c => c.DeliverCompany)
                .Include(c => c.Nationality)
                .Include(c => c.User)
                .ThenInclude(c => c.city)
                .Include(c => c.RenewCompany)
                .Where(c => c.Id == orderId)
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            return MapOrderToOrderDetailsDtoForCompanies(order, currentDate, cultureInfo, lang);
        }
        private OrderDetailsDto MapOrderToOrderDetailsDto(Order order, DateTime currentDate, CultureInfo cultureInfo, string lang)
        {
            var orderInfo = MapOrderInfo(order, currentDate, cultureInfo, lang);
            var orderCarInfo = MapOrderCarInfo(order, lang);
            var orderDeliveryCompanyInfo = MapOrderDeliveryCompanyInfo(order, lang);
            var paymentInfo = MapPaymentInfo(order, lang);
            var userInfo = MapUserInfo(order, lang);
            var renewOrderInfo = MapRenewOrderInfo(order, lang);

            return new OrderDetailsDto
            {
                orderInfo = orderInfo,
                OrderCarInfo = orderCarInfo,
                orderDeliveryCompanyInfo = orderDeliveryCompanyInfo,
                paymentInfo = paymentInfo,
                userInfo = userInfo,
                renewOrderInfo = renewOrderInfo
            };
        }
        private OrderDetailsDto MapOrderToOrderDetailsDtoForCompanies(Order order, DateTime currentDate, CultureInfo cultureInfo, string lang)
        {
            var orderInfo = MapOrderInfo(order, currentDate, cultureInfo, lang);
            var orderCarInfo = MapOrderCarInfo(order, lang);
            var orderDeliveryCompanyInfo = MapOrderDeliveryCompanyInfo(order, lang);
            var paymentInfo = MapPaymentInfoForCompanies(order, lang);
            var userInfo = MapUserInfo(order, lang);
            var renewOrderInfo = MapRenewOrderInfo(order, lang);

            return new OrderDetailsDto
            {
                orderInfo = orderInfo,
                OrderCarInfo = orderCarInfo,
                orderDeliveryCompanyInfo = orderDeliveryCompanyInfo,
                paymentInfo = paymentInfo,
                userInfo = userInfo,
                renewOrderInfo = renewOrderInfo
            };
        }
        private InfoOrderDto MapOrderInfo(Order order, DateTime currentDate, CultureInfo cultureInfo, string lang)
        {
            var isRenew = order.HasRenewToAdditionalPeriod && order.RenewOrderdata != null && order.RenewOrderdata.Count() > 0;
            var renewOrder = isRenew ? order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault() : null;
            var orderInfo = new InfoOrderDto
            {
                orderId = order.Id,

                orderCaseText = isRenew ? Helper.OrderCaseText(order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().Case, lang)
                                 : Helper.OrderCaseText(order.OrderCase, lang),

                orderStatusText = Helper.StutesText(order.OrderStatus, lang),

                orderCase = isRenew ? order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().Case
                           : order.OrderCase,

                orderStatus = order.OrderStatus,

                price = isRenew ? renewOrder.NetPrice + renewOrder.BreakPeriodPrice + renewOrder.RentalConfirmationDelayPrice : order.NetPrice + order.RentalConfirmationDelayPrice + order.BreakPeriodPrice,

                date = order.CreationDate.Humanize(true, currentDate, cultureInfo),

                orderDate = order.CreationDate.ToString("dd/MM/yyyy", cultureInfo),

                canceledText = order.ReasonToCancled,

                rejectionType = order.RejectionType != null ? Helper.GetRejectionTypeText(order.RejectionType.Value, lang) : "",

                remaingDateToEndRental = isRenew ? (order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().DateEndRentalPeriod - currentDate).Days
                : (order.DateEndRentalPeriod - currentDate).Days,

                renewAndCancelContractText = _db.Settings.Select(c => c.ChangeLangInformationRenewalOrcCancellation(lang)).FirstOrDefault(),
                isRenewToAnotherPeriod = order.HasRenewToAdditionalPeriod

            };
            return orderInfo;
        }
        private CarInfoDto MapOrderCarInfo(Order order, string lang)
        {
            var isRenew = order.HasRenewToAdditionalPeriod && order.RenewOrderdata != null && order.RenewOrderdata.Count() > 0;
            var confirmRecvieCar = order.IsConfirmReciptCar;
            var carInfo = new CarInfoDto
            {
                rentalCompanyName = order.RenewCompany.user_Name,
                datePickupCar = order.DatePickupCar != null ? order.DatePickupCar.Value.ToString("dd/MM/yyyy") : "",

                timePickupCar = order.TimePickupCar != null ? order.TimePickupCar.Value.ToString("hh:mm tt") : "",

                dateDeliveryCar = confirmRecvieCar ? order.DateReceiptCar.ToString("dd/MM/yyyy") : isRenew
                           ? order.RenewOrderdata
                                  .OrderByDescending(c => c.Id).FirstOrDefault().
                                   DateEndRentalPeriod.ToString("dd/MM/yyyy")
                           : order.DateEndRentalPeriod.ToString("dd/MM/yyyy"),

                //timeDeliveryCar = isRenew
                //           ? order.RenewOrderdata.FirstOrDefault(c => c.Status == (int)OrderStutes.Current).DateEndRentalPeriod.ToString("hh:mm tt")
                //           : order.DateEndRentalPeriod.ToString("hh:mm tt"),
                timeDeliveryCar = confirmRecvieCar ? order.DateReceiptCar.ToString("hh:mm tt") : order.TimePickupCar != null ? order.TimePickupCar.Value.ToString("hh:mm tt") : "",

                rentalPeriod = order.RentalPeriod,

                dateStartPeriod = order.DateRentalPeriod.ToString("dd/MM/yyyy"),

                locationPickupCar = order.LocationpickupCar ?? "",

                lat = order.lat ?? "",
                lng = order.lng ?? "",

                carCategory = lang == "ar" ? _db.CarCategories.FirstOrDefault(ca => ca.Id == order.OrderCar.CarCategoryId)?.NameAr :
                               _db.CarCategories.FirstOrDefault(ca => ca.Id == order.OrderCar.CarCategoryId)?.NameEn,

                carModel = lang == "ar" ? _db.CarModels.FirstOrDefault(ca => ca.Id == order.OrderCar.CarModelId)?.NameAr :
                                       _db.CarModels.FirstOrDefault(ca => ca.Id == order.OrderCar.CarModelId)?.NameEn,

                carType = lang == "ar" ? _db.CarTypes.FirstOrDefault(ca => ca.Id == order.OrderCar.CarTypeId)?.NameAr :
                                       _db.CarTypes.FirstOrDefault(ca => ca.Id == order.OrderCar.CarTypeId)?.NameEn,

                //newRentalPeriod = isRenew? order.RenewOrderdata.OrderByDescending(c => c.Id).Select(c => c.Duration).FirstOrDefault():0,

                finishedRentalPeriod = order.DateEndRentalPeriod.Date <= DateTime.Now,

                insurancInformation = order.OrderCar.InsuranceInformation != null ? order.OrderCar.InsuranceInformation : "",

                fileInsurancInformation = order.OrderCar.FileInsurancInformation != null ? DashBordUrl.baseUrlHost + order.OrderCar.FileInsurancInformation : "",

                carForm = order.OrderCar.CarForm != null ? DashBordUrl.baseUrlHost + order.OrderCar.CarForm : ""
            };
            return carInfo;
        }
        private List<DeliverCompanyOrderInfoDto> MapOrderDeliveryCompanyInfo(Order order, string lang)
        {
            var deliverCompany = order.DeliverCompany.Where(d => d.DeliverCompany != null)
                    //.Where(d => d.DeliverCompany != null && (order.OrderStatus == (int)OrderStutes.NewOrder ? true :
                    //d.DeliverCompanyCase != (int)OrderCase.WaitToAcceptDeliverCompany))
                    .Select(d => new DeliverCompanyOrderInfoDto
                    {
                        deliveryCompanyId = d.DeliverCompanyId,

                        companyName = d.DeliverCompany.user_Name,

                        reasonRefused = d.ReasonRefused,

                        loginData = _db.DataForDeliverApps
                            .FirstOrDefault(da => da.UserId == order.UserId && da.DeliverCompanyId == d.DeliverCompanyId && da.OrderId == order.Id)?.LoginData ?? "",


                        password = _db.DataForDeliverApps
                            .FirstOrDefault(da => da.UserId == order.UserId && da.DeliverCompanyId == d.DeliverCompanyId && da.OrderId == order.Id)?.Password ?? "",

                        orderLoginData = d.UserDataInAppDelivery,
                        isConfirmLoginData = d.ConfirmDataOfDeliveryApp,
                        isRefused = d.ReasonRefused != null,
                        driverTypeInDliveryCompanyText = Helper.GetDeliveryCompanyTypeText(d.TypeDriverInDeliveryCompanies, lang),
                        driverTypeInDliveryCompany = d.TypeDriverInDeliveryCompanies,
                        orderCompnayCase = d.DeliverCompanyCase
                    }).ToList();
            return deliverCompany;
        }
        private OrderPaymentInfoDto MapPaymentInfo(Order order, string lang)
        {
            var isRenew = order.HasRenewToAdditionalPeriod && order.RenewOrderdata != null && order.RenewOrderdata.Count() > 0;
            RenewOrderdata reneworderInfo=new RenewOrderdata();
            if (isRenew)
            {
             reneworderInfo = order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault();

            }
            var paymentInfo = new OrderPaymentInfoDto
            {
                newPeriod= isRenew ? reneworderInfo.Duration:0,
                varPrice = isRenew? reneworderInfo.VatPrice: order.VatPrice,

                vatPercetage = isRenew? reneworderInfo.VatPercetage: order.VatPercetage,

                priceForDailyRental = isRenew? order.OrderCar.RentalPriceDaily : order.OrderCar.RentalPriceDaily,

                discountPrice = isRenew ? reneworderInfo.coponPrice??0:order.coponPrice ?? 0,

                subTotal = isRenew? reneworderInfo.subTotal: order.subTotal,

                patmentType = isRenew? Helper.GetPaymentText(reneworderInfo.TypePay, lang) : Helper.GetPaymentText(order.TypePay, lang),

                FinishOrderOrderPeriod = isRenew ? reneworderInfo.Case == (int)OrderCase.FinishedRentalPeriodAndNotRenew
                                                ? (DateTime.UtcNow.AddHours(3) - reneworderInfo.DateEndRentalPeriod).Days : 0
                    : order.OrderCase == (int)OrderCase.FinishedRentalPeriodAndNotRenew
                                     ? (DateTime.UtcNow.AddHours(3) - order.DateEndRentalPeriod).Days : 0,

                PriceFinishOrderOrderPeriod = isRenew ? reneworderInfo.Case == (int)OrderCase.FinishedRentalPeriodAndNotRenew
                                                    ? ((DateTime.UtcNow.AddHours(3) - reneworderInfo.DateEndRentalPeriod).Days) * reneworderInfo.DiscountValueForDailyRental
                                                    : 0:
                    order.OrderCase == (int)OrderCase.FinishedRentalPeriodAndNotRenew
                                             ? ((DateTime.UtcNow.AddHours(3) - order.DateEndRentalPeriod).Days) * order.DiscountValueForDailyRental
                                             : 0,
                breakPeriod = isRenew?reneworderInfo.BreakPeriod: order.BreakPeriod,

                breakPrice = isRenew ? reneworderInfo.BreakPeriodPrice:order.BreakPeriodPrice,

                dailyRentalCompanyPeriod = isRenew ? reneworderInfo.RentalConfirmationDelayPeriod : order.RentalConfirmationDelayPeriod,

                dailyRentalCompanyPrice = isRenew ? reneworderInfo.RentalConfirmationDelayPrice : order.RentalConfirmationDelayPrice,

                netPrice = isRenew ? reneworderInfo.NetPrice + reneworderInfo.RentalConfirmationDelayPrice + reneworderInfo.BreakPeriodPrice : order.NetPrice + order.RentalConfirmationDelayPrice + order.BreakPeriodPrice,
            };
            return paymentInfo;
        }
        private OrderPaymentInfoDto MapPaymentInfoForCompanies(Order order, string lang)
        {
         
            var paymentInfo = new OrderPaymentInfoDto
            {
                newPeriod = 0,
                varPrice = order.VatPrice,

                vatPercetage =  order.VatPercetage,

                priceForDailyRental =  order.OrderCar.RentalPriceDaily,

                discountPrice =order.coponPrice ?? 0,

                subTotal = order.subTotal,

                patmentType = Helper.GetPaymentText(order.TypePay, lang),

                FinishOrderOrderPeriod =  order.OrderCase == (int)OrderCase.FinishedRentalPeriodAndNotRenew
                                     ? (DateTime.UtcNow.AddHours(3) - order.DateEndRentalPeriod).Days : 0,

                PriceFinishOrderOrderPeriod =   order.OrderCase == (int)OrderCase.FinishedRentalPeriodAndNotRenew
                                             ? ((DateTime.UtcNow.AddHours(3) - order.DateEndRentalPeriod).Days) * order.DiscountValueForDailyRental
                                             : 0,
                breakPeriod =  order.BreakPeriod,

                breakPrice =  order.BreakPeriodPrice,

                dailyRentalCompanyPeriod =  order.RentalConfirmationDelayPeriod,

                dailyRentalCompanyPrice =  order.RentalConfirmationDelayPrice,

                netPrice =  order.NetPrice + order.RentalConfirmationDelayPrice + order.BreakPeriodPrice,
            };
            return paymentInfo;
        }
        private OrderUserInfoDto MapUserInfo(Order order, string lang)
        {
            var userInfo = new OrderUserInfoDto
            {
                userId = order.UserId,
                age = order.Age,
                identityNumber = order.IdentityNumber,
                deliverLicenceImage = DashBordUrl.baseUrlHost + order.DrivingLicenseImage,
                indentityNumberImage = DashBordUrl.baseUrlHost + order.IdentityImage,
                image = DashBordUrl.baseUrlHost + order.User.ImgProfile,
                genderType = Helper.GetGenderTypeText(order.GenderType, lang),
                email = order.User.Email,
                phoneNumber = order.User.PhoneCode + " " + order.User.PhoneNumber,
                user_Name = order.User.user_Name,
                city = order.User.city.ChangeLangName(lang),
                nationality = order.Nationality.ChangeLangName(lang),
                genderTypeId = order.GenderType,
                nationalityId = order.Nationality.Id
            };
            return userInfo;
        }
        private RenewOrderInfoDto MapRenewOrderInfo(Order order, string lang)
        {
            var isRenew = order.HasRenewToAdditionalPeriod && order.RenewOrderdata != null && order.RenewOrderdata.Count() > 0;
            var renewOrderInfoDto = isRenew ? order.RenewOrderdata
                    .OrderByDescending(c => c.Id)
                    .Select(c => new RenewOrderInfoDto
                    {
                        newPeriod = c.Duration,


                        FinishOrderPeriod = c.Case == (int)OrderCase.FinishedRentalPeriodAndNotRenew
                                                ? (DateTime.UtcNow.AddHours(3) - c.DateEndRentalPeriod).Days : 0,

                        PriceFinishOrderPeriod = c.Case == (int)OrderCase.FinishedRentalPeriodAndNotRenew
                                                    ? ((DateTime.UtcNow.AddHours(3) - c.DateEndRentalPeriod).Days) * c.DiscountValueForDailyRental
                                                    : 0,
                        discountPrice = c.subTotal - c.NetPrice,
                        subTotal = c.subTotal,
                        varPrice = c.VatPrice,
                        vatPercetage = c.VatPercetage,

                        breakPeriod = c.BreakPeriod,

                        breakPrice = c.BreakPeriodPrice,

                        dailyRentalCompanyPeriod = c.RentalConfirmationDelayPeriod,

                        dailyRentalCompanyPrice = c.RentalConfirmationDelayPrice,
                        netPrice = c.NetPrice + c.RentalConfirmationDelayPrice + c.BreakPeriodPrice,
                    }).FirstOrDefault()
                      : null;
            return renewOrderInfoDto!;
        }

        #endregion
        #region Corn Jop

        public async Task<bool> FinishedOrderNotApprovalByRentalCompany()
        {
            var permissiblePeriodToEndOrder = await _db.Settings
                 .Select(c => c.PermissiblePeriod)
                 .FirstOrDefaultAsync();
            var currentdate = DateTime.UtcNow.AddHours(3);

            var orders = await _db.Orders
                .Where(c => c.OrderCase == (int)OrderCase.WaitToAcceptRentalCompany
                && c.AdminAprovalDate.AddDays(permissiblePeriodToEndOrder) < currentdate).ToListAsync();
            foreach (var order in orders)
            {
                order.OrderStatus = (int)OrderStutes.Refused;
                order.OrderCase = (int)OrderCase.Refused;
            }
            _db.UpdateRange(orders);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> FinishedOrderNotApprovalByDeliveryCompany()
        {
            var permissiblePeriodToEndOrder = await _db.Settings
           .Select(c => c.PermissiblePeriod)
           .FirstOrDefaultAsync();

            var currentdate = DateTime.UtcNow.AddHours(3);

            var orders = await _db.Orders
                .Where(c => c.OrderCase == (int)OrderCase.WaitToAcceptDeliverCompany
                       && c.RentalCompanyAprovalDate.AddDays(permissiblePeriodToEndOrder) < currentdate).ToListAsync();
            foreach (var order in orders)
            {
                order.OrderStatus = (int)OrderStutes.Refused;
                order.OrderCase = (int)OrderCase.Refused;
            }
            _db.UpdateRange(orders);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancelOrderNotApproval()
        {
            var currentdate = DateTime.UtcNow.AddHours(3);

            var orders = await _db.Orders
                .Where(c => c.OrderStatus==(int)OrderStutes.NewOrder&&currentdate.Date>c.DateRentalPeriod.Date).ToListAsync();
            foreach (var order in orders)
            {
                order.OrderStatus = (int)OrderStutes.Refused;
                order.OrderCase = (int)OrderCase.Refused;
            }
            _db.UpdateRange(orders);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ChechEndRentalPeriod()
        {
            var orders = await _db.Orders
                .Include(c => c.RenewCompany)
                .Include(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Where(c => c.OrderStatus == (int)OrderStutes.Current && c.OrderCase != (int)OrderCase.FinishedRentalPeriodAndNotRenew
                    && c.DateEndRentalPeriod <= DateTime.UtcNow.AddHours(3) && !c.HasRenewToAdditionalPeriod)
                .ToListAsync();
            var renewData = await _db.RenewOrderdata
                .Include(c => c.Order)
                .ThenInclude(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Include(c => c.Order)
                .ThenInclude(c => c.RenewCompany)
                .Where(c => c.Status == (int)OrderStutes.Current && c.Case != (int)OrderCase.FinishedRentalPeriodAndNotRenew && c.DateEndRentalPeriod <= DateTime.UtcNow.AddHours(3))
               .ToListAsync();
            foreach (var order in orders)
            {
                order.OrderCase = (int)OrderCase.FinishedRentalPeriodAndNotRenew;
            }
            foreach (var order in renewData)
            {
                order.Case = (int)OrderCase.FinishedRentalPeriodAndNotRenew;
            }
            _db.RenewOrderdata.UpdateRange(renewData);
            _db.Orders.UpdateRange(orders);
            await _db.SaveChangesAsync();
            var notifyUsers = new List<NotifyUser>();
            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            foreach (var order in orders)
            {
                notifyUsers.Add(new NotifyUser
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = $"تم انتهاء مدة الاستئجار للطلب رقم {order.Id} برجاء التوجه إلى شركة التأجير {order.RenewCompany.user_Name} لتسليم السيارة أو التجديد",
                    TextEn = $"The rental period for order number {order.Id} has ended. Please visit the rental company {order.RenewCompany.user_Name} to return the car or renew the rental.",
                    Type = (int)NotifyTypes.AcceptOrderByAdmin
                });
                var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.UserId,
                    ProjectName = x.ProjectName
                }).ToList();
                NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
               , order.User.Lang == "ar" ? $"تم انتهاء مدة الاستئجار للطلب رقم {order.Id} برجاء التوجه إلى شركة التأجير{order.RenewCompany.user_Name} لتسليم السيارة أو التجديد" : $"The rental period for order number {order.Id} has ended. Please visit the rental company {order.RenewCompany.user_Name} to return the car or renew the rental.", (int)NotifyTypes.EndRentalPeriod, order.Id, (int)OrderStutes.Current);
            }
            foreach (var renew in renewData)
            {
                notifyUsers.Add(new NotifyUser
                {
                    OrderId = renew.Order.Id,
                    UserId = renew.Order.UserId,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = $"تم انتهاء مدة الاستئجار للطلب رقم {renew.Order.Id} برجاء التوجه إلى شركة التأجير {renew.Order.RenewCompany.user_Name} لتسليم السيارة أو التجديد",
                    TextEn = $"The rental period for order number {renew.Order.Id} has ended. Please visit the rental company  {renew.Order.RenewCompany.user_Name} to return the car or renew the rental.",
                    Type = (int)NotifyTypes.EndRentalPeriod
                });
                var deviceIds = renew.Order.User.DeviceId.Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.UserId,
                    ProjectName = x.ProjectName
                }).ToList();
                NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
               , renew.Order.User.Lang == "ar" ? $"تم انتهاء مدة الاستئجار للطلب رقم {renew.Order.Id} برجاء التوجه إلى شركة التأجير {renew.Order.RenewCompany.user_Name} لتسليم السيارة أو التجديد" : $"The rental period for order number {renew.Order.Id} has ended. Please visit the rental company {renew.Order.RenewCompany.user_Name} to return the car or renew the rental.", (int)NotifyTypes.EndRentalPeriod, renew.Order.Id, (int)OrderStutes.Current);
            }
            _db.NotifyUsers.UpdateRange(notifyUsers);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CalculateBreakingPeriodAndPrice()
        {
            await ChechEndRentalPeriod();
            var orders = await _db.Orders
                      .Where(c => c.OrderCase == (int)OrderCase.FinishedRentalPeriodAndNotRenew)
                         .ToListAsync();


            var currentdate = HelperDate.GetCurrentDate();

            var isBreakPeriod = false;
            int breakPeriod = 0;
            foreach (var order in orders)
            {
                order.OrderCase = (int)OrderCase.FinishedRentalPeriodAndNotRenew;
                isBreakPeriod = currentdate.Date > order.DateEndRentalPeriod.Date;
                if (isBreakPeriod)
                {
                    breakPeriod = (currentdate.Date - order.DateEndRentalPeriod.Date).Days;
                    order.BreakPeriod = breakPeriod;

                    order.BreakPeriodPrice = GetPriceForRentalCarDaily(order, breakPeriod) * breakPeriod;
                }
            }


            var renewOrders = await _db.RenewOrderdata
                .Include(c => c.Order)
                         .Where(c => c.Case == (int)OrderCase.FinishedRentalPeriodAndNotRenew)
                         .ToListAsync();
            foreach (var renew in renewOrders)
            {
                renew.Case = (int)OrderCase.FinishedRentalPeriodAndNotRenew;
                isBreakPeriod = currentdate.Date > renew.DateEndRentalPeriod.Date;
                if (isBreakPeriod)
                {
                    breakPeriod = (currentdate.Date - renew.DateEndRentalPeriod.Date).Days;
                    renew.BreakPeriod = breakPeriod;

                    renew.BreakPeriodPrice = GetPriceForRentalCarDaily(renew.Order, breakPeriod) * breakPeriod;
                }
            }
            _db.RenewOrderdata.UpdateRange(renewOrders);
            _db.Orders.UpdateRange(orders);
            await _db.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
