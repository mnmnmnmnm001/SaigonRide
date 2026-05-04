using SaigonRide.Models;

namespace SaigonRide.Services
{
    public interface IPaymentService
    {
        bool ProcessPayment(User user, double amount, string paymentMethod);
        List<string> GetAvailablePaymentMethods(User user);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IFareCalculationService _fareCalculationService;

        public PaymentService(IFareCalculationService fareCalculationService)
        {
            _fareCalculationService = fareCalculationService;
        }

        public bool ProcessPayment(User user, double amount, string paymentMethod)
        {
            if (user == null || amount <= 0)
                return false;

            // Validate payment method is available for user
            var availableMethods = GetAvailablePaymentMethods(user);
            if (!availableMethods.Contains(paymentMethod))
                return false;

            // Simulate payment processing
            if (user is LocalCommuter local)
            {
                return ProcessLocalPayment(local, amount, paymentMethod);
            }
            else if (user is ForeignTourist tourist)
            {
                return ProcessTouristPayment(tourist, amount, paymentMethod);
            }

            return false;
        }

        public List<string> GetAvailablePaymentMethods(User user)
        {
            var methods = new List<string> { "Cash" };

            if (user is LocalCommuter local)
            {
                if (local.P_MoMo)
                    methods.Add("MoMo");
                if (local.P_VNPay)
                    methods.Add("VNPay");
            }
            else if (user is ForeignTourist tourist)
            {
                if (tourist.P_ApplePay)
                    methods.Add("ApplePay");
                if (tourist.P_PayPal)
                    methods.Add("PayPal");
            }

            return methods;
        }

        private bool ProcessLocalPayment(LocalCommuter local, double amount, string paymentMethod)
        {
            // Simulate local payment processing
            if (paymentMethod.ToLower() == "momo" && local.P_MoMo)
            {
                local.Payed += amount;
                return true;
            }
            else if (paymentMethod.ToLower() == "vnpay" && local.P_VNPay)
            {
                local.Payed += amount;
                return true;
            }
            else if (paymentMethod.ToLower() == "cash")
            {
                local.Payed += amount;
                return true;
            }

            return false;
        }

        private bool ProcessTouristPayment(ForeignTourist tourist, double amount, string paymentMethod)
        {
            // Validate passport for international payments
            if (string.IsNullOrEmpty(tourist.Passport))
                return false;

            // Simulate tourist payment processing
            if (paymentMethod.ToLower() == "applepay" && tourist.P_ApplePay)
            {
                tourist.Payed += amount;
                return true;
            }
            else if (paymentMethod.ToLower() == "paypal" && tourist.P_PayPal)
            {
                tourist.Payed += amount;
                return true;
            }
            else if (paymentMethod.ToLower() == "cash")
            {
                tourist.Payed += amount;
                return true;
            }

            return false;
        }
    }
}
