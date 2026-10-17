using EventFlowAPI.DB.Entities.Abstract;

namespace EventFlowAPI.Logic.Helpers
{
    public class PaymentTypeInfo : BaseEntity, INameableEntity, IPhotoEntity
    {
        public string Name { get; set; } = string.Empty;
        public string PhotoName { get; set; } = string.Empty;
    }
}
