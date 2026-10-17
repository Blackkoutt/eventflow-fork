using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class FestivalSeedExtentions
    {
        public static void Seed(this EntityTypeBuilder<Festival> entityBuilder, DateTime today)
        {
            entityBuilder.HasData(
                new Festival
                {
                    Id = 1,
                    Name = "Festiwal Muzyki Hip-Hop",
                    ShortDescription = "Największy festiwal hip-hopowy! Koncerty, freestyle battle, warsztaty DJ-skie i spotkania z artystami.",
                    StartDate = today.AddMonths(1).AddDays(1),
                    EndDate = today.AddMonths(2).AddDays(1).AddHours(1),
                    FestivalGuid = new Guid("0e36fd43-030a-4459-88fa-f99f53e601a3"),
                    PhotoName = "festiwal_muzyki_hip_hop.png",
                    LongDescription = "Festiwal Muzyki Hip-Hop to coroczne wydarzenie dedykowane nowoczesnym" +
                    " brzmieniom i eksperymentalnym kompozycjom." +
                    " Na scenie pojawiają się zarówno uznani artyści, jak i młode talenty," +
                    " prezentując innowacyjne podejście do muzyki. W programie znajdują się koncerty," +
                    " warsztaty, spotkania z twórcami oraz panele dyskusyjne dotyczące przyszłości muzyki." +
                    " Festiwal przyciąga miłośników dźwięków elektronicznych, jazzowych improwizacji, minimalistycznych" +
                    " kompozycji i nowych form ekspresji dźwiękowej. To wyjątkowa okazja, by odkryć najnowsze trendy muzyczne" +
                    " i poszerzyć swoje horyzonty artystyczne."
                },
                new Festival
                {
                    Id = 2,
                    Name = "Festiwal Filmowy",
                    ShortDescription = "Przegląd najlepszych filmów z całego świata! Premiera, spotkania z reżyserami i nocne maratony kinowe.",
                    StartDate = today.AddMonths(1).AddDays(2),
                    EndDate = today.AddMonths(2).AddDays(3).AddHours(2),
                    FestivalGuid = new Guid("d946406d-7b80-40ca-aeff-c2966dcb6727"),
                    PhotoName = "festiwal_filmowy.png",
                    LongDescription = "Festiwal Filmowy to wyjątkowe święto kina, które gromadzi pasjonatów filmów" +
                    " z różnych zakątków świata. W repertuarze znajdują się zarówno filmy fabularne, dokumentalne," +
                    " jak i krótkometrażowe, reprezentujące różne nurty i stylistyki. Organizowane są spotkania z reżyserami," +
                    " aktorami oraz warsztaty dla młodych filmowców. Każda edycja festiwalu poświęcona jest określonemu" +
                    " tematowi przewodniemu, który inspiruje twórców do dyskusji nad istotnymi kwestiami społecznymi," +
                    " kulturowymi i artystycznymi. To doskonała okazja, by zobaczyć premiery, odkryć nowe talenty i zanurzyć się" +
                    " w magiczny świat kinematografii."
                },

                new Festival
                {
                    Id = 3,
                    Name = "Festiwal Sztuki",
                    ShortDescription = "Wystawy, instalacje i performanse! Odkryj nowoczesne formy sztuki i weź udział w kreatywnych warsztatach.",
                    StartDate = today.AddMonths(1).AddDays(4),
                    EndDate = today.AddMonths(2).AddDays(4).AddHours(3),
                    FestivalGuid = new Guid("d96a3633-6a7f-42fb-ba84-94e5902adce7"),
                    PhotoName = "festiwal_sztuki.png",
                    LongDescription = "Festiwal Sztuki to wydarzenie celebrujące kreatywność" +
                    " i nieograniczoną ekspresję artystyczną. W ramach festiwalu prezentowane są prace artystów," +
                    " którzy eksperymentują z formą, kolorem i strukturą, tworząc unikalne dzieła sztuki nowoczesnej." +
                    " Oprócz wystaw odbywają się warsztaty, podczas których uczestnicy mogą spróbować swoich sił w malarstwie," +
                    " rzeźbie czy instalacjach multimedialnych. Festiwal to także przestrzeń do rozmów o współczesnych" +
                    " trendach w sztuce, spotkania z artystami oraz wykłady ekspertów. To niepowtarzalna okazja, by odkryć" +
                    " nowe inspiracje i spojrzeć na sztukę z innej perspektywy."
                }
            );
        }
    }
}
