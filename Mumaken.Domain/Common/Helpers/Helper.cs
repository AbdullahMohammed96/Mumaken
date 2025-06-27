using Mumaken.Domain.Enums;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QRCoder;
using System.Drawing;
using static Mumaken.Domain.Common.PathUrl.DashBordUrl;
using AAITHelper;
using Mumaken.Domain.Entities.UserTables;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Org.BouncyCastle.Security;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.SettingDto;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace Mumaken.Domain.Common.Helpers
{
    public class Helper : IHelper
    {
        private readonly IWebHostEnvironment HostingEnvironment;
        private IConverter _converter;
        private readonly IConfiguration _configuration;
        public static string translateFolderPath { get; set; } = null;

        public Helper(IConverter converter, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            HostingEnvironment = hostingEnvironment;
            _converter = converter;
            _configuration = configuration;
        }


        public string ValidateImage(IFormFile? image)
        {
            if (image != null)
            {
                var allowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };

                if (!allowedExtensions.Contains(Path.GetExtension(image.FileName).ToLower()))
                {
                    return "غير مسموح سوي بالامتدادت التالية: JPG، PNG, JPEG";
                }

                if (image.Length > 1048576)
                {
                    return "لا يمكنك رفع صورة حجمها اكبر من 1 ميجابايت";
                }
            }
            return string.Empty;
        }
        public string Upload(IFormFile Photo, int FileName)
        {
            string folderName = System.Enum.GetName(typeof(Enums.FileName), FileName);
            string uniqueFileName = string.Empty;
            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(HostingEnvironment.WebRootPath, $"images/{folderName}");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Photo.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return $"images/{folderName}/" + uniqueFileName;
        }
        public async Task<string> UploadFileUsingApi(UploadImageDto model)
        {
            //var apiUrl = "https://localhost:7293/api/v1/UploadImage";
            var apiUrl = $"{DashBordUrl.baseUrlHost}api/v1/UploadImage";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var formData = new MultipartFormDataContent();

                    // Use the stream without explicitly disposing it
                    var fileContent = new StreamContent(model.image.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                    formData.Add(fileContent, "image", model.image.FileName);

                    formData.Add(new StringContent(model.fileName.ToString()), "fileName");

                    var response = await httpClient.PostAsync($"{apiUrl}", formData);

                    string jsonString = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeAnonymousType(jsonString, new { result = "" });
                    return responseObject.result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static string RemoveHtmlTags(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Regular expression to match HTML tags
            string pattern = "<.*?>";
            return Regex.Replace(input, pattern, string.Empty);
        }
        public string GenerateToken(ApplicationDbUser user)
        {
            var generatedToken = HelperGeneral.GetToken(_configuration, user.Id, user.TypeUser.ToString(), user.user_Name);
            var token = new JwtSecurityTokenHandler().WriteToken(generatedToken);
            return token;
        }
        #region Roles

        public string GetRole(string role, string lang)
        {
            return role switch
            {
                "AdminBranch" => AAITHelper.HelperMsg.creatMessage(lang, "سوبر ادمن", "AdminBranch"),
                "Admin" => AAITHelper.HelperMsg.creatMessage(lang, "أدمن", "Admin"),
                "Mobile" => AAITHelper.HelperMsg.creatMessage(lang, "موبايل", "Mobile"),
                "SocialMedia" => AAITHelper.HelperMsg.creatMessage(lang, "مواقع التواصل الاجتماعى", "SocialMedia"),
                "Notifications" => AAITHelper.HelperMsg.creatMessage(lang, "الاشعارات", "Notifications"),
                "ContactUs" => AAITHelper.HelperMsg.creatMessage(lang, "تواصل معنا", "ContactUs"),
                "Setting" => AAITHelper.HelperMsg.creatMessage(lang, "الاعدادات", "Setting"),
                "Clients" => AAITHelper.HelperMsg.creatMessage(lang, "مستخدمي التطبيق", "Clients"),
                "Cities" => AAITHelper.HelperMsg.creatMessage(lang, "المدن", "Cities"),
                "Regoins" => AAITHelper.HelperMsg.creatMessage(lang, "الاحياء", "Regoins"),
                "ProductReport" => AAITHelper.HelperMsg.creatMessage(lang, "البلاغات", "ProductReport"),
                "FinancialAccounts" => AAITHelper.HelperMsg.creatMessage(lang, "الحسابات المالية", "FinancialAccounts"),
                "CarType" => AAITHelper.HelperMsg.creatMessage(lang, " ماركات السيارة", "CarType"),
                "CarModel" => AAITHelper.HelperMsg.creatMessage(lang, " موديلات السيارة", "CarModel"),
                "CarCategory" => AAITHelper.HelperMsg.creatMessage(lang, " فئات السيارة", "CarCategory"),
                "DeliveryCompany" => AAITHelper.HelperMsg.creatMessage(lang, "شركات التوصيل", " DeliveryCompany"),
                "Sliders" => AAITHelper.HelperMsg.creatMessage(lang, "الاعلانات", " Sliders"),
                "OrderCycleIntroScreen" => AAITHelper.HelperMsg.creatMessage(lang, "صفحات تعریفیة", " OrderCycleIntroScreen"),
                "Copon" => AAITHelper.HelperMsg.creatMessage(lang, "الكوبونات", " Copon"),
                "RentalCompany" => AAITHelper.HelperMsg.creatMessage(lang, "شركات التوصيل", " CopoRentalCompanyn"),
                "Orders" => AAITHelper.HelperMsg.creatMessage(lang, "الطلبات", " Orders"),
                "IntroScreens" => AAITHelper.HelperMsg.creatMessage(lang, "صفحات البداية", " IntroScreens"),
                "CommonQuestion" => AAITHelper.HelperMsg.creatMessage(lang, "الاسئلة الشائعة", " CommonQuestion"),
                "Banks" => AAITHelper.HelperMsg.creatMessage(lang, "البنوك", " Banks"),
                _ => role
            };
        }

        #endregion
        public static string paymentOrderText(int PaymentOrder,string lang="ar")
        {
            string text = "";
            switch (PaymentOrder)
            {
                case (int)PaymentOrderType.PaymentOrderToEndRentalPeriod:
                    text = (lang == "ar" ? "تم الدفع وانتهاء مده التاجير" : "Payment has been made and the rental period has expired");
                    break;

                case (int)PaymentOrderType.CancelContract:
                    text = (lang == "ar" ? "تم الدفع والغاء التعاقد بنجاح" : "Payment was made and the contract was canceled successfully");
                    break;
                case (int)PaymentOrderType.PaymentAndRenewPeriod:
                    text = (lang == "ar" ? "تم الدفع وتجديد المدة بنجاح" : "Payment and renewal of the term has been completed successfully");
                    break;
            }
            return text;
        }
        public static string StutesText(int stutes, string lang = "ar")
        {
            string text = "";
            switch (stutes)
            {
                case (int)OrderStutes.NewOrder:
                    text = (lang == "ar" ? "طلب جديد" : "New Order");
                    break;
                case (int)OrderStutes.Current:
                    text = (lang == "ar" ? "طلب حالي" : "Current Order");
                    break;
                case (int)OrderStutes.Finished:
                    text = (lang == "ar" ? "طلب منتهي" : "Finished Order");
                    break;
                case (int)OrderStutes.Canceled:
                    text = (lang == "ar" ? "طلب ملغي" : "Canceled Order");
                    break;
            }
            return text;
        }
        public static string OrderCaseText(int stutes, string lang = "ar")
        {
            string text = "";
            switch (stutes)
            {
                case (int)OrderCase.WaitAcceptAdministration:
                    text = (lang == "ar" ? " في انتظار موافقه الادارة" : "Waiting for management approval ");
                    break;
                case (int)OrderCase.WaitToAcceptRentalCompany:
                    text = (lang == "ar" ? " في انتظار موافقه شركه التاجير" : "Waiting for approval from the rental company");
                    break;
                case (int)OrderCase.WaitToAcceptDeliverCompany:
                    text = (lang == "ar" ? " في انتظار موافقه شركه التوصيل" : "Waiting for approval from the delivery company");
                    break;
                case (int)OrderCase.Accepted:
                    text = (lang == "ar" ? " تم قبول الطلب" : "The Oder has been accepted");
                    break;
                case (int)OrderCase.FinishedRentalPeriodAndNotRenew:
                    text = (lang == "ar" ? " تم انتهاء مده الاستئجار ولم يتم التجديد" : "The rental period has ended And Not Renew");
                    break;
                case (int)OrderCase.CancelContractAndPaidRentalPeriod:
                    text = (lang == "ar" ? "تم إلغاء التعاقد ودفع قيمة الاستئجار" : "The contract has been canceled and the rental price has been paid");
                    break;
                case (int)OrderCase.CancelContractByAdminAndNotPaidRentalPeriod:
                    text = (lang == "ar" ? "تم إلغاء التعاقد  من قبل  الادارة ولم يتم  دفع قيمة الاستئجار" : "تم إلغاء التعاقد  من قبل  الادارة ولم يتم  دفع قيمة الاستئجار");
                    break;
                case (int)OrderCase.CancelContractByAdminAndPaidRentalPeriod:
                    text = (lang == "ar" ? "تم إلغاء التعاقد  من قبل  الادارة دفع قيمة الاستئجار" : "The contract was canceled by the administration and the rental value was paid");
                    break;
                case (int)OrderCase.RenewRentalPeriod:
                    text = (lang == "ar" ? " تم تجديد مده الاستئجار" : "The rental period has been renewed");
                    break;
                case (int)OrderCase.Finished:
                    text = (lang == "ar" ? "تم انتهاء الطلب" : "The Oder has been Finished");
                    break;
                case (int)OrderCase.Canceled:
                    text = (lang == "ar" ? "تم رفض الطلب بواسطه العميل" : "The request was rejected by the Client");
                    break;
                case (int)OrderCase.SendRequsetToCancelContract:
                    text = (lang == "ar" ? "تم ارسال طلب الغاء التعاقد برجاء التوجه الي شركه التاجير لتسليم السيارة" : "A request to cancel the contract has been sent. Please go to the rental company to deliver the car");
                    break;
                case (int)OrderCase.SendRequsetToFinishedContract:
                    text = (lang == "ar" ? "تم ارسال طلب إنهاء التعاقد برجاء التوجه الي شركه التاجير لتسليم السيارة" : "A request to terminate the contract has been sent. Please go to the rental company to deliver the car");
                    break;
                case (int)OrderCase.ConfirmReciptCar:
                    text = (lang == "ar" ? "تم تاكيد استلام السيارة من قبل شركة التأجير الرجاء تسديد مبلغ الايجار" : "The car has been confirmed by the rental company. Please pay the rental amount");
                    break;
                case (int)OrderCase.PaymentOrderAndWaitConfirmRentanlCompany:
                    text = (lang == "ar" ? "تم دفع قيمه الاستئجار بنجاح في انتظار تاكيد شركة التاجير لانهاء الطلب" : "The rental price has been paid successfully, awaiting confirmation from the rental company to complete the request");
                    break;
                case (int)OrderCase.Refused:
                    text = (lang == "ar" ? "تم رفض الطلب" : "The Oder has been Canceled");
                    break;
            }
            return text;
        }

        public static string OrderCaseRentalCompanyText(int ordercase, string lang = "ar")
        {
            string text = "";
            switch (ordercase)
            {
                case (int)OrderCase.WaitToAcceptRentalCompany:
                    text = (lang == "ar" ? " في انتظار موافقه شركه التاجير" : "Waiting for approval from the rental company");
                    break;
                case (int)OrderCase.WaitToAcceptDeliverCompany:
                    text = (lang == "ar" ? " في انتظار موافقه شركه التوصيل" : "Waiting for approval from the delivery company");
                    break;
                case (int)OrderCase.FinishedRentalPeriodAndNotRenew:
                    text = (lang == "ar" ? " تم انتهاء مده الاستئجار ولم يتم التجديد" : "The rental period has ended And Not Renew");
                    break;
                case (int)OrderCase.RenewRentalPeriod:
                    text = (lang == "ar" ? "تم دفع قيمة الايجار وتجديد المده" : "The rent has been paid and the term has been renewed");
                    break;
                case (int)OrderCase.Finished:
                    text = (lang == "ar" ? "تم تاكيد استلام قيمة مبلغ الاستئجار" : "The receipt of the rental amount has been confirmed");
                    break;
                case (int)OrderCase.Canceled:
                    text = (lang == "ar" ? "تم رفض الطلب بواسطه العميل" : "The request was rejected by the Client");
                    break;
                case (int)OrderCase.SendRequsetToCancelContract:
                    text = (lang == "ar" ? " طلب  العميل الغاء التعاقد  وتم إخطاره بالتوجه الي تسليم السيارة " : "The Client requested to cancel the contract and was notified to go to deliver the car");
                    break;
                case (int)OrderCase.SendRequsetToFinishedContract:
                    text = (lang == "ar" ? "انتهت مده الاستئجار وطلب العميل انهاء التعاقد وتم إخطاره بالتوجه الي تسليم السيارة" : "The rental period has expired, and the customer requested to terminate the contract and was notified to go to deliver the car");
                    break;
                case (int)OrderCase.ConfirmReciptCar:
                    text = (lang == "ar" ? "تم تاكيد استلام السيارة من في انتظار تسديد العميل لقيمه مدة الاستئجار" : "The car has been confirmed and the customer is waiting for payment for the rental period");
                    break;
                case (int)OrderCase.PaymentOrderAndWaitConfirmRentanlCompany:
                    text = (lang == "ar" ? "تم تاكيد استلام السيارة بنجاح في انتظار  تسديد العميل لقيمة مدة الاستئجار" : "The car has been successfully received, pending the customer's payment for the rental period");
                    break;
                case (int)OrderCase.Refused:
                    text = (lang == "ar" ? "تم رفض الطلب" : "The Oder has been Canceled");
                    break;
            }
            return text;
        }
        public static string GetGenderTypeText(int genderType, string lang = "ar")
        {
            string text = "";
            switch (genderType)
            {
                case (int)GenderType.male:
                    text = (lang == "ar" ? "ذكر" : "male");
                    break;

                case (int)GenderType.female:
                    text = (lang == "ar" ? "انثي" : "female");
                    break;

            }
            return text;
        }
        public static string GetPaymentText(int paymentType, string lang = "ar")
        {
            string text = "";
            switch (paymentType)
            {
                case (int)TypePay.online:
                    text = (lang == "ar" ? "الكتروني" : "Online");
                    break;

                case (int)TypePay.cash:
                    text = (lang == "ar" ? "كاش" : "Cash");
                    break;
                case (int)TypePay.walet:
                    text = (lang == "ar" ? "محفظه" : "Walet");
                    break;
            }
            return text;
        }
        public static string GetRejectionTypeText(int paymentType, string lang = "ar")
        {
            string text = "";
            switch (paymentType)
            {
                case (int)RejectionType.RejectionFromAdmin:
                    text = (lang == "ar" ? "رفض من الادارة" : "Rejected by the administration");
                    break;

                case (int)RejectionType.RejectionFromRentalCompany:
                    text = (lang == "ar" ? "رفض من شركات التاجير" : "Rejected by the Rental Company");
                    break;
                case (int)RejectionType.RejectionFromDeliveryCompany:
                    text = (lang == "ar" ? "رفض  من شركات التوصيل" : "Rejected by the Delivery Company");
                    break;
            }
            return text;
        }
        public static string GetDeliveryCompanyTypeText(int type, string lang = "ar")
        {
            string text = "";
            switch (type)
            {
                case (int)TypeDriverInDeliveryCompanies.Joining:
                    text = (lang == "ar" ? "إنضمام" : "Joining");
                    break;

                case (int)TypeDriverInDeliveryCompanies.Update:
                    text = (lang == "ar" ? "تحديث" : "Update");
                    break;
            }
            return text;
        }
        public static string GetTypeAddUserText(int type, string lang = "ar")
        {
            string text = "";
            switch (type)
            {
                case (int)AddedUserType.AddByMobile:
                    text = (lang == "ar" ? "فرد مستقل" : "Independent individual");
                    break;

                case (int)AddedUserType.AddDeliverCompany:
                    text = (lang == "ar" ? "تابع لشركة توصيل" : "Affiliated with a delivery company");
                    break;
            }
            return text;
        }
        public static List<SelectListItem> OrderStatusSelectList(string lang="ar")
        {
            List<SelectListItem> orderStutasText = new List<SelectListItem>()
            {

                new SelectListItem {Text = lang=="ar"?"جديد":"New", Value = "1"},
                new SelectListItem {Text = lang=="ar"?"حالي":"Current", Value = "2"},
                new SelectListItem {Text = lang=="ar"?"منتهي":"Finished", Value = "3"},
                new SelectListItem {Text = lang=="ar"?"ملغي":"Canceled", Value = "4"},

            };
            return orderStutasText;
        }
        public static List<SelectListItem> OrderCaseSelectList(string lang="ar")
        {
            List<SelectListItem> orderStutasText = new List<SelectListItem>()
            {

                new SelectListItem {Text =lang=="ar"?"جديد":"New", Value = "1"},
                new SelectListItem {Text =lang=="ar"? "حالي":"Current", Value = "2"},
                new SelectListItem {Text =lang=="ar"? "طلبات التجديد":"Renewal Orders", Value = "3"},
                new SelectListItem {Text =lang=="ar"? "طلبات إلغاء التعاقد":"Contract cancellation Orders", Value = "4"},
                new SelectListItem {Text =lang=="ar"? "منتهي":"Finished", Value = "5"},


            };
            return orderStutasText;
        }
        public static List<SelectListItem> OrderDeliveyCompanyCaseSelectList(string lang="ar")
        {
            List<SelectListItem> orderStutasText = new List<SelectListItem>()
            {

                new SelectListItem {Text =lang=="ar" ?"جديد":"New", Value = "3"},
                new SelectListItem {Text =lang=="ar" ? "حالي":"Current", Value = "6"},
                new SelectListItem {Text =lang=="ar" ? "منتهي":"Finished", Value = "7"},


            };
            return orderStutasText;
        }
        public static CultureInfo GetCulture(string lang)
        {
            return lang == "ar" ? new CultureInfo("ar-EG") : new CultureInfo("en-US");
        }

        public static string GetLanguage()
        {
            string lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            return lang;
        }
        public static bool isImage(IFormFile image)
        {
            var allowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };
            var result = allowedExtensions.Contains(Path.GetExtension(image.FileName).ToLower());
            return result;
        }
        #region QrCode
        public static byte[] GenerateQrcode(string textCode)
        {
            byte[] QrCode = null;

            if (textCode != null)
            {
                QRCodeGenerator qr = new QRCodeGenerator();
                QRCodeData data = qr.CreateQrCode(textCode, QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);

                Bitmap bitMap = code.GetGraphic(20);
                QrCode = BitmapToBytesCode(bitMap);
                return QrCode;
            }
            return QrCode;
        }

        private static byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        #endregion

        #region Generate Pdf
        public string CreatePDF(string controllerAction, int id)
        {
            var pdfname = String.Format("{0}.pdf", id);
            var globalSettings = new GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = Path.Combine(HostingEnvironment.WebRootPath, "pdf", pdfname)
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,

                //بياخد  لينك اى صفحة لتحويلها ل pdf

                Page = $"{baseUrlHost}{controllerAction}/{id}"

            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }

            };
            _converter.Convert(pdf);

            //var myfile = System.IO.File.ReadAllBytes($"wwwroot/pdf/{pdfname}");

            //return File(myfile, System.Net.Mime.MediaTypeNames.Application.Pdf);

            //return globalSettings.Out;
            return $"pdf/{id}.pdf";
        }
        #endregion


        public static string getFileTranslate(Lang.Translate lang, string key)
        {
            //var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"Common/Translate.json");
            //var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"..\\Mumaken.Domain\\Common\\Translate.json");


            if (translateFolderPath == null)
            {
                try
                {
                    translateFolderPath = Path.Combine(Directory.GetCurrentDirectory(), $"Common\\Translate.json");
                    File.ReadAllText(translateFolderPath);
                }
                catch (Exception ex)
                {
                    translateFolderPath = Path.Combine(Directory.GetCurrentDirectory(), $"..\\Mumaken.Domain\\Common\\Translate.json");
                    File.ReadAllText(translateFolderPath);
                }
            }

            var JSON = File.ReadAllText(translateFolderPath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(JSON);

            dynamic mainRoot = jsonObj[key];
            string text = mainRoot[lang.ToString()].ToString();
            return text;





        }

        //public static async Task<bool> SendNotifyAsync(ApplicationDbContext db, string textAr, string textEn, string fkProvider, int stutes)
        //{
        //    var user = await db.Users.Where(x => x.Id == fkProvider).FirstOrDefaultAsync();
        //    if (user.TypeUser == (int)Enums.AllEnums.User_Type.Client)
        //    {
        //        NotifyClient notifyClient = new NotifyClient()
        //        {
        //            Date = HelperDate.GetCurrentDate(),
        //            FKUser = fkProvider,
        //            Show = false,
        //            TextAr = textAr,
        //            TextEn = textEn,
        //            Type = stutes
        //        };
        //        await db.NotifyClients.AddAsync(notifyClient);
        //        await db.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        NotifyDelegt notifyDelegt = new NotifyDelegt()
        //        {
        //            Date = HelperDate.GetCurrentDate(),
        //            FKUser = fkProvider,
        //            Show = false,
        //            TextAr = textAr,
        //            TextEn = textEn,
        //            Type = stutes
        //        };
        //        await db.NotifyDelegts.AddAsync(notifyDelegt);
        //        await db.SaveChangesAsync();
        //    }
        //    var Fcm = await db.Settings.FirstOrDefaultAsync();

        //    var AllDeviceids = await db.DeviceIds.Where(x => x.FkUser == fkProvider).Select(x => new DeviceIdModel()
        //    {
        //        DeviceId = x.DeviceId_,
        //        DeviceType = x.DeviceType,
        //        FkUser = x.FkUser,
        //        ProjectName = x.ProjectName

        //    }).ToListAsync();
        //    HelperFcm.SendPushNotification(Fcm.ApplicationId, Fcm.SenderId, AllDeviceids, null, textAr, stutes);
        //    return true;
        //}



    }
}
