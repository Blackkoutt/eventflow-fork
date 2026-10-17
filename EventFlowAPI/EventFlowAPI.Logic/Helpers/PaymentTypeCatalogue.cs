using EventFlowAPI.DB.Entities.Enums;

namespace EventFlowAPI.Logic.Helpers
{
    public static class PaymentTypeCatalogue
    {
        public static readonly PaymentTypeInfo PayU = new() { Id = (int)PaymentType.PayU, Name = "PayU" };
        public static readonly PaymentTypeInfo Pass = new() { Id = (int)PaymentType.Pass, Name = "Karnet" };
        public static readonly IReadOnlyList<PaymentTypeInfo> All = [PayU, Pass];
    }
}
