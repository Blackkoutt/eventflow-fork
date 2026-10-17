using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Query.Abstract;

namespace EventFlowAPI.Logic.Query
{
    public class PartnerQuery : QueryObject, INameableQuery
    {
        public string? Name { get; set; }
        public PartnerKind? Kind { get; set; }
    }
}
