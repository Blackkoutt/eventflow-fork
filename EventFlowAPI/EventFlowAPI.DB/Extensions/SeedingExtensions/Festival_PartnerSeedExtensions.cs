using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class Festival_PartnerSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Festival_Partner> entityBuilder)
        {
            entityBuilder.HasData(
                new Festival_Partner { FestivalId = 1, PartnerId = 1 },
                new Festival_Partner { FestivalId = 1, PartnerId = 2 },
                new Festival_Partner { FestivalId = 2, PartnerId = 1 },
                new Festival_Partner { FestivalId = 2, PartnerId = 3 },
                new Festival_Partner { FestivalId = 3, PartnerId = 2 },
                new Festival_Partner { FestivalId = 3, PartnerId = 3 },
                new Festival_Partner { FestivalId = 1, PartnerId = 4 },
                new Festival_Partner { FestivalId = 1, PartnerId = 5 },
                new Festival_Partner { FestivalId = 2, PartnerId = 4 },
                new Festival_Partner { FestivalId = 2, PartnerId = 6 },
                new Festival_Partner { FestivalId = 3, PartnerId = 5 },
                new Festival_Partner { FestivalId = 3, PartnerId = 6 },
                new Festival_Partner { FestivalId = 1, PartnerId = 7 },
                new Festival_Partner { FestivalId = 1, PartnerId = 8 },
                new Festival_Partner { FestivalId = 2, PartnerId = 7 },
                new Festival_Partner { FestivalId = 2, PartnerId = 9 },
                new Festival_Partner { FestivalId = 3, PartnerId = 8 },
                new Festival_Partner { FestivalId = 3, PartnerId = 9 }
            );
        }
    }
}
