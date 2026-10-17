namespace EventFlowAPI.DB.Entities
{
    public static class PaymentTypes
    {
        public static readonly PaymentType PayU = new() { Id = 1, Name = "PayU" };
        public static readonly PaymentType Pass = new() { Id = 2, Name = "Karnet" };
        public static readonly IReadOnlyList<PaymentType> All = [PayU, Pass];
    }
}
