using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class EventSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Event> entityBuilder, DateTime today)
        {
            entityBuilder.HasData(
                new Event
                {
                    Id = 1,
                    Name = "Koncert: Mystic Waves",
                    ShortDescription = "Mystic Waves to niezapomniana podróż przez elektroniczne brzmienia i hipnotyzujące melodie.",
                    StartDate = today.AddMonths(1).AddDays(1),
                    EndDate = today.AddMonths(1).AddDays(1).AddHours(1),
                    DurationTimeSpan = today.AddMonths(1).AddDays(1).AddHours(1) - today.AddMonths(1).AddDays(1),
                    CategoryId = 1,
                    HallId = 6,
                    EventGuid = new Guid("bd9cf606-260b-4611-bb6f-3cde4298edc7"),
                    PhotoName = "koncert_mystic_waves.png"
                },
                new Event
                {
                    Id = 2,
                    Name = "Cień Przeszłości",
                    ShortDescription = "Dramatyczna opowieść o sekretach przeszłości, które powracają, by zmienić teraźniejszość.",
                    StartDate = today.AddMonths(1).AddDays(2),
                    EndDate = today.AddMonths(1).AddDays(2).AddHours(3),
                    DurationTimeSpan = today.AddMonths(1).AddDays(2).AddHours(3) - today.AddMonths(1).AddDays(2),
                    CategoryId = 3,
                    HallId = 7,
                    EventGuid = new Guid("30f24949-51df-4274-a804-544ee7e2e1d2"),
                    PhotoName = "cien_przeszlosci.png"
                },
                new Event
                {
                    Id = 3,
                    Name = "Królestwo planety małp",
                    ShortDescription = "Kolejna część kultowej serii sci-fi. Świat, w którym dominują małpy, a ludzie walczą o przetrwanie.",
                    StartDate = today.AddMonths(1).AddDays(3),
                    EndDate = today.AddMonths(1).AddDays(3).AddHours(2),
                    DurationTimeSpan = today.AddMonths(1).AddDays(3).AddHours(2) - today.AddMonths(1).AddDays(3),
                    CategoryId = 2,
                    HallId = 5,
                    EventGuid = new Guid("63b9ba64-48dd-4997-95c9-dbfa2389f066"),
                    PhotoName = "krolestwo_planety_malp.png"
                },
                new Event
                {
                    Id = 4,
                    Name = "Nowe inspiracje",
                    ShortDescription = "Wystawa prezentująca prace nowoczesnych artystów, pełna świeżych perspektyw i innowacji.",
                    StartDate = today.AddMonths(1).AddDays(4),
                    EndDate = today.AddMonths(1).AddDays(4).AddHours(3),
                    DurationTimeSpan = today.AddMonths(1).AddDays(4).AddHours(3) - today.AddMonths(1).AddDays(4),
                    CategoryId = 4,
                    HallId = 8,
                    EventGuid = new Guid("5e01e785-8f63-4f8c-af89-25ee89f22b83"),
                    PhotoName = "nowe_inspiracje.png"
                },
                new Event
                {
                    Id = 5,
                    Name = "Koncert: New Era",
                    ShortDescription = "Energetyczny koncert zespołu New Era – mieszanka rocka, elektroniki i alternatywnych brzmień.",
                    StartDate = today.AddMonths(2).AddDays(1),
                    EndDate = today.AddMonths(2).AddDays(1).AddHours(1),
                    DurationTimeSpan = today.AddMonths(2).AddDays(1).AddHours(1) - today.AddMonths(2).AddDays(1),
                    CategoryId = 1,
                    HallId = 10,
                    EventGuid = new Guid("7f940a53-f3a2-46d8-8d39-b28e076d40aa"),
                    PhotoName = "new_era.png"
                },
                new Event
                {
                    Id = 6,
                    Name = "Gladiator",
                    ShortDescription = "Legendarna opowieść o honorze i zemście w starożytnym Rzymie.",
                    StartDate = today.AddMonths(2).AddDays(3),
                    EndDate = today.AddMonths(2).AddDays(3).AddHours(2),
                    DurationTimeSpan = today.AddMonths(2).AddDays(3).AddHours(2) - today.AddMonths(2).AddDays(3),
                    CategoryId = 2,
                    HallId = 9,
                    EventGuid = new Guid("bc7b2f85-b498-4186-ad5c-5cf25d938499"),
                    PhotoName = "gladiator.png"
                },
                new Event
                {
                    Id = 7,
                    Name = "Nowa sztuka",
                    ShortDescription = "Wystawa prezentująca najnowsze prace artystów eksperymentalnych i awangardowych.",
                    StartDate = today.AddMonths(2).AddDays(4),
                    EndDate = today.AddMonths(2).AddDays(4).AddHours(3),
                    DurationTimeSpan = today.AddMonths(2).AddDays(4).AddHours(3) - today.AddMonths(2).AddDays(4),
                    CategoryId = 4,
                    HallId = 11,
                    EventGuid = new Guid("775b339d-4524-4829-bd42-4da8f88cf3f1"),
                },
                new Event
                {
                    Id = 8,
                    Name = "Cień Nad Miastem",
                    ShortDescription = "Mroczny thriller teatralny o tajemniczych wydarzeniach, które wstrząsają miastem po zmroku.",
                    StartDate = today.AddMonths(2).AddDays(15),
                    EndDate = today.AddMonths(2).AddDays(15).AddHours(2).AddMinutes(30),
                    DurationTimeSpan = today.AddMonths(2).AddDays(15).AddHours(2).AddMinutes(30) - today.AddMonths(2).AddDays(15),
                    CategoryId = 3,
                    HallId = 16,
                    EventGuid = new Guid("08050835-f7b3-4395-9679-b7bc915c84a6"),
                    PhotoName = "cien_nad_miastem.png"
                },
                new Event
                {
                    Id = 9,
                    Name = "Jazzowe Brzmienia",
                    ShortDescription = "Magiczny czas z jazzem na żywo – koncert znanych artystów jazzowych, którzy przeniosą Cię w świat wyjątkowych dźwięków.",
                    StartDate = today.AddDays(-1).AddHours(1),
                    EndDate = today.AddDays(-1).AddHours(3),
                    DurationTimeSpan = today.AddDays(-1).AddHours(3) - today.AddDays(-1).AddHours(1),
                    CategoryId = 1,
                    HallId = 17,
                    EventGuid = new Guid("a4f24bff-10c1-4763-b410-ba0e6e7bb3d3"),
                    PhotoName = "jazz.png"
                },
                new Event
                {
                    Id = 10,
                    Name = "Interstellar",
                    ShortDescription = "Epicka podróż kosmiczna, która bada granice ludzkiej wytrzymałości i miłości.",
                    StartDate = today.AddDays(-2).AddHours(2),
                    EndDate = today.AddDays(-2).AddHours(4),
                    DurationTimeSpan = today.AddDays(-2).AddHours(4) - today.AddDays(-2).AddHours(2),
                    CategoryId = 2,
                    HallId = 18,
                    EventGuid = new Guid("f9b817c5-e386-4107-9ed3-49b7ce38c519"),
                    PhotoName = "interstellar.jpg"
                },
                new Event
                {
                    Id = 11,
                    Name = "Zatrzymać Przeznaczenie",
                    ShortDescription = "Spektakl o postaci, która staje w obliczu nieuchronnego przeznaczenia.",
                    StartDate = today.AddDays(-3).AddHours(3),
                    EndDate = today.AddDays(-3).AddHours(5),
                    DurationTimeSpan = today.AddDays(-3).AddHours(5) - today.AddDays(-3).AddHours(3),
                    CategoryId = 3,
                    HallId = 19,
                    EventGuid = new Guid("ef97fdf1-9b04-47aa-bc42-1ff4f7fd5a07"),
                    PhotoName = "zatrzymac_przeznaczenie.jpg"
                },
                new Event
                {
                    Id = 12,
                    Name = "Nowe Perspektywy",
                    ShortDescription = "Wystawa sztuki współczesnej, która zaprasza do odkrywania świata z nowych punktów widzenia.",
                    StartDate = today.AddDays(-4).AddHours(4),
                    EndDate = today.AddDays(-4).AddHours(6),
                    DurationTimeSpan = today.AddDays(-4).AddHours(6) - today.AddDays(-4).AddHours(4),
                    CategoryId = 4,
                    HallId = 20,
                    EventGuid = new Guid("3c229abc-c4e2-41e9-9aad-487cd733f8a5"),
                    PhotoName = "nowe_perspektywy.jpg"
                }
            );
        }
    }
}
