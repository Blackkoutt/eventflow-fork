using EventFlowAPI.DB.Entities.Abstract;

namespace EventFlowAPI.DB.Entities
{
    public class PaymentType : BaseEntity, INameableEntity, IPhotoEntity
    {
        public string Name { get; set; } = string.Empty;
        public string PhotoName { get; set; } = string.Empty;
    }
}
