namespace SaigonRide.Models
{
    public abstract class User
    {
        public int UserId { get; set; }
        public string BankNum { get; set; }
        public string ChosenPaymentCode { get; set; }
        public double Payed { get; set; }
        public string UserType { get; set; } // "LocalCommuter" or "ForeignTourist"
    }

    public class LocalCommuter : User
    {
        public LocalCommuter()
        {
            UserType = "LocalCommuter";
        }

        public bool P_MoMo { get; set; }
        public bool P_VNPay { get; set; }
    }

    public class ForeignTourist : User
    {
        public ForeignTourist()
        {
            UserType = "ForeignTourist";
        }

        public string Passport { get; set; }
        public bool P_ApplePay { get; set; }
        public bool P_PayPal { get; set; }
    }
}
