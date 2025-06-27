namespace Mumaken.Domain.Common.PathUrl
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class General
        {
            public const string HomePage = Base + "/HomePage";
            public const string GetOrderSplash = Base + "/GetOrderSplash";
            public const string GetNationalities = Base + "/GetNationalities";
            public const string GetCars = Base + "/GetCars";
            public const string GetAllCarCatrgory = Base + "/GetAllCarCatrgory";
            public const string GetAllCarType = Base + "/GetAllCarType";
            public const string GetAllCarModelWithCarType = Base + "/GetAllCarModelWithCarType";
            public const string GetAllCities = Base + "/GetAllCities";
            public const string GetAllDeliveryCompanies = Base + "/GetAllDeliveryCompanies";
            public const string Filter = Base + "/Filter";
        }
        public static class More
        {
            public const string GetIntroScreen = Base + "/GetIntroScreen";
            public const string GetAboutApp = Base + "/GetAboutApp";
            public const string GetProvicyPolicy = Base + "/GetProvicyPolicy";
            public const string GetClientConditionAndTerm = Base + "/GetClientConditionAndTerm";
            public const string GetClientAboutUs = Base + "/GetClientAboutUs";
            public const string GetCommonQuestion = Base + "/GetCommonQuestion";
            public const string GetContactInfo = Base + "/GetContactInfo";
            public const string AddContactUs = Base + "/AddContactUs";
            public const string GetAllCity = Base + "/GetAllCity";
            public const string UploadImage = Base + "/UploadImage";
        }
        public static class OrderApi
        {
            public const string TestNotifcation = Base + "/TestNotifcation";
            public const string AddNewOrder = Base + "/AddNewOrder";
            public const string GetOrderPaymentInfo = Base + "/GetOrderPaymentInfo";

            public const string UseCopon = Base + "/UseCopon";
            public const string GetOrderbyStatus = Base + "/GetOrderbyStatus";
            public const string GetOrderDetails = Base + "/GetOrderDetails";
            public const string GetOrderPriceForCancleContact = Base + "/GetOrderPriceForCancleContact";
            public const string CancelContarct = Base + "/CancelContarct";
            public const string FinishContarct = Base + "/FinishContarct";
            public const string ConfirmReciptCar = Base + "/ConfirmReciptCar";
            public const string PayRentalPeriod = Base + "/PayRentalPeriod";
            public const string RenewOrderToAnotherPeriod = Base + "/RenewOrderToAnotherPeriod";
            public const string CanceledOrder = Base + "/CanceledOrder";
            public const string UseCoponInOrder = Base + "/UseCoponInOrder";
            public const string PaymentOrder = Base + "/PaymentOrder";
            public const string RenewalRentalPeriod = Base + "/RenewalRentalPeriod";
            public const string AddDeliveryCompanyToCurrentOrder = Base + "/AddDeliveryCompanyToCurrentOrder";
            public const string ChechEndRentalPeriod = Base + "/ChechEndRentalPeriod";
            public const string FinishedOrderNotApprovalByRentalCompany = Base + "/FinishedOrderNotApprovalByRentalCompany";
            public const string FinishedOrderNotApprovalByDeliveryCompany = Base + "/FinishedOrderNotApprovalByDeliveryCompany";
            public const string CalculateBreakingPeriodAndPrice = Base + "/CalculateBreakingPeriodAndPrice";
            public const string CancelOrderNotApproval = Base + "/CancelOrderNotApproval";
        }

        public static class Identity
        {

            public const string GetDataOfProvider = Base + "/GetDataOfProvider";
            public const string UpdateAsyncDataDelegt = Base + "/UpdateAsyncDataDelegt";
            public const string RegisterClient = Base + "/RegisterClient";
            public const string RegisterDeleget = Base + "/RegisterDeleget";
            public const string RemoveNotiyByIdAsync = Base + "/RemoveNotiyByIdAsync";
            public const string RemoveAllNotify = Base + "/RemoveAllNotify";
            public const string RemoveNotifyById = Base + "/RemoveNotifyById";
            public const string UpdateForWebsiteAsyncDataUser = Base + "/UpdateForWebsiteAsyncDataUser";
            public const string RemoveAccount = Base + "/RemoveAccount";
            // client
            public const string ResendCode = Base + "/ResendCode";
            public const string ListOfNotifyUser = Base + "/ListOfNotifyUser";
            public const string ChangeReciveOrder = Base + "/ChangeReciveOrder";
            public const string UpdateDataUser = Base + "/UpdateDataUser";
            public const string ChangePasswordByCode = Base + "/ChangePasswordByCode";
            public const string Login = Base + "/login";
            public const string ForgetPassword = Base + "/ForgetPassword";
            public const string CheckCode = Base + "/CheckCode";
            public const string Register = Base + "/register";
            public const string ListCity = Base + "/ListCity";
            public const string ConfirmCodeRegister = Base + "/ConfirmCodeRegister";
            public const string reset_password = Base + "/reset_password";
            public const string ChangePassward = Base + "/ChangePassward";
            public const string ChangePhoneNumber = Base + "/ChangePhoneNumber";
            public const string ConfirmChangePhoneDto = Base + "/ConfirmChangePhoneDto";
            public const string ChangeEmail = Base + "/ChangeEmail";
            public const string ConfirmChangeEmail = Base + "/ConfirmChangeEmail";
            public const string GetDataOfUser = Base + "/GetDataOfUser";
            public const string GetUserWalet = Base + "/GetUserWalet";
            public const string ConvertloyaltytoWallet = Base + "/ConvertloyaltytoWallet";

            // addtional services from user 
            public const string ChangeLanguage = Base + "/ChangeLanguage";
            public const string ChangeNotify = Base + "/ChangeNotify";
            public const string Logout = Base + "/Logout";
            public const string VerifyName = Base + "/VerifyName";
            public const string SendSMS = Base + "/SendSMS";
        }

        public static class Chat
        {
            public const string ListMessagesUser = Base + "/ListMessagesUser";
            public const string UploadNewFile = Base + "/UploadNewFile";
            public const string ListUsersMyChat = Base + "/ListUsersMyChat";
        }

    }
}
