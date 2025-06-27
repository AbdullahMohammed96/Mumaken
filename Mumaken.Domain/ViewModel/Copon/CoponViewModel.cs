namespace Mumaken.Domain.ViewModel.Copon
{
    public class CoponViewModel
    {
        public int Id { get; set; }

        public int count { get; set; }// عدد المستفيدين 

        public int countUsed { get; set; }// عدد استخدام الكوبون 
        public string expirdate { get; set; }// تاريخ انتهاء الخصم
        public string coponCode { get; set; } // 

        public double discount { get; set; }// نسبه ياعنى 20% 

        public double limtdiscount { get; set; } //حد اقصى للخصم مثلا 50 ريال
        public bool isActive { get; set; }// يعامل معامله الحذف 
        //public string rentalCompanyName { get; set; }
    }
}
