namespace AAITPayment.HyperPay.Model
{/// <summary>
/// موديل الدفع الخاص بهايبر باي Hyperpay
/// </summary>
    public class PaymentModel
    {
        public bool IsLive { get; set; } = false;
        public bool IsMada { get; set; } = false;
        /// <summary>
        /// خاص ب بيانات العميل نحصل عليه من ايميل هايبر باي
        /// 
        /// و يكون خاص بخدمة مدى MADA
        /// </summary>
        public string MadaEntityId { get; set; } = "";
        /// <summary>
        /// خاص ب بيانات العميل نحصل عليه من ايميل هايبر باي
        /// 
        /// و يكون خاص بخدمة فيزا او ماستر كارد VISA - MASTERCARD
        /// </summary>
        public string ViMaEntityId { get; set; } = "8ac7a4ca7d14ab59017d1daa7cbb0f38";
        public string amount { get; set; } = "10";
        public string givenName { get; set; } = "Hasan";
        public string surname { get; set; } = "Darwish";
        public string email { get; set; } = "hasan-dr2010@hotmail.com";
        public string street1 { get; set; } = "test st.";
        public string city { get; set; } = "mekkah";
        public string state { get; set; } = "madina";
        public string postcode { get; set; } = "12005";
        /// <summary>
        /// الدولة
        /// 
        /// لازم يكون بصيغة alpha-2 code
        /// 
        /// مثال : السعودية - SA
        /// 
        /// https://www.iban.com/country-codes
        /// </summary>
        public string country { get; set; } = "SA";
        /// <summary>
        /// العملة المستخدمة في الدفع 
        /// 
        /// منجيبها من ايميل هايبر باي
        /// 
        /// ريال سعودي - SAR
        /// </summary>
        public string currency { get; set; } = "SAR";
        /// <summary>
        /// طريقة الدفع
        /// 
        /// منجيبها من ايميل هايبر باي
        /// 
        /// مثال : DB
        /// 
        /// DEBIT CARD
        /// </summary>
        public string paymentType { get; set; } = "DB";
        /// <summary>
        /// التوكن الخاصة بحساب العميل - live
        /// 
        /// منجيبها من ايميل هايبر باي
        /// </summary>
        public string LiveAccessToken { get; set; } = "";
        /// <summary>
        /// التوكن الخاصة بحساب العميل - test
        /// 
        /// منجيبها من ايميل هايبر باي
        /// </summary>
        public string TestAccessToken { get; set; } = "OGFjN2E0Y2E3ZDE0YWI1OTAxN2QxZGE5ODk2MTBmMzR8QUFSNEtHdEVyag==";
        public string UserId { get; set; } = "0741b669-781e-455d-bddb-96e2ea97ab77";
        /// <summary>
        /// طريقة الدفع الاونلاين
        /// 
        /// من enum OnlinePaymentType
        /// 
        /// 1 - فيزا او ماستر كارد
        /// 
        /// 2 - مدى
        /// 
        /// 3 - ابل باي
        /// </summary>
        public int PaymentMethod { get; set; } = 1;

    }
}
