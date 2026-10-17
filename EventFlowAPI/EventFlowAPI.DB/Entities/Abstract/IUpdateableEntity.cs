namespace EventFlowAPI.DB.Entities.Abstract
{
    public interface IUpdateableEntity
    {
        bool IsUpdated { get; set; }
        DateTime? UpdateDate { get; set; }
    }
}
