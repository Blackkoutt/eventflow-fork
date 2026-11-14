using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class HallType_EquipmentSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<HallType_Equipment> entityBuilder)
        {
            entityBuilder.HasData(
                new HallType_Equipment
                {
                    HallTypeId = 1,
                    EquipmentId = 1
                },
                new HallType_Equipment
                {
                    HallTypeId = 1,
                    EquipmentId = 3
                },
                new HallType_Equipment
                {
                    HallTypeId = 1,
                    EquipmentId = 7
                },


                new HallType_Equipment
                {
                    HallTypeId = 2,
                    EquipmentId = 2
                },
                new HallType_Equipment
                {
                    HallTypeId = 2,
                    EquipmentId = 4
                },
                new HallType_Equipment
                {
                    HallTypeId = 2,
                    EquipmentId = 5
                },
                new HallType_Equipment
                {
                    HallTypeId = 2,
                    EquipmentId = 7
                },


                new HallType_Equipment
                {
                    HallTypeId = 3,
                    EquipmentId = 2
                },
                new HallType_Equipment
                {
                    HallTypeId = 3,
                    EquipmentId = 7
                },
                new HallType_Equipment
                {
                    HallTypeId = 3,
                    EquipmentId = 9
                },


                new HallType_Equipment
                {
                    HallTypeId = 4,
                    EquipmentId = 1
                },
                new HallType_Equipment
                {
                    HallTypeId = 4,
                    EquipmentId = 7
                },
                new HallType_Equipment
                {
                    HallTypeId = 4,
                    EquipmentId = 10
                },
                new HallType_Equipment
                {
                    HallTypeId = 4,
                    EquipmentId = 11
                },
                

                new HallType_Equipment
                {
                    HallTypeId = 5,
                    EquipmentId = 1
                },
                new HallType_Equipment
                {
                    HallTypeId = 5,
                    EquipmentId = 5
                },
                new HallType_Equipment
                {
                    HallTypeId = 5,
                    EquipmentId = 6
                },
                new HallType_Equipment
                {
                    HallTypeId = 5,
                    EquipmentId = 7
                },


                new HallType_Equipment
                {
                    HallTypeId = 6,
                    EquipmentId = 1
                },
                new HallType_Equipment
                {
                    HallTypeId = 6,
                    EquipmentId = 5
                },
                new HallType_Equipment
                {
                    HallTypeId = 6,
                    EquipmentId = 6
                },
                new HallType_Equipment
                {
                    HallTypeId = 6,
                    EquipmentId = 7
                },
                new HallType_Equipment
                {
                    HallTypeId = 6,
                    EquipmentId = 8
                }
            );
        }
    }
}
