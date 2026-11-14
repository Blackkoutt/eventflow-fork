using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class HallTypeSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<HallType> entityBuilder)
        {
            entityBuilder.HasData(
                new HallType
                {
                    Id = 1,
                    Name = "Sala filmowa",
                    Description = "Nowoczesna sala kinowa, wyposażona w wysokiej jakości projektor oraz system nagłośnienia Dolby Digital. Idealna do projekcji filmów w różnych formatach, zapewniająca doskonałe wrażenia wizualne i dźwiękowe. Pomieszczenie jest przystosowane do projekcji 2D i 3D, z przestronnymi fotelami zapewniającymi komfort podczas długotrwałego seansu.",
                    PhotoName = "filmowa.jpg"
                },

                new HallType
                {
                    Id = 2,
                    Name = "Sala koncertowa",
                    Description = "Sala koncertowa o najwyższej jakości akustyce, przeznaczona do organizacji koncertów, recitali i występów muzycznych. Wyposażona w profesjonalny system nagłośnienia i adaptację akustyczną, która zapewnia doskonałe warunki dźwiękowe dla artystów oraz publiczności. Przestronny design, komfortowe siedzenia oraz nowoczesne oświetlenie sprawiają, że jest to idealne miejsce do wydarzeń muzycznych.",
                    PhotoName = "koncertowa.jpg"
                },

                new HallType
                {
                    Id = 3,
                    Name = "Sala teatralna",
                    Description = "Sala teatralna o wysokiej jakości akustyce i estetyce, przystosowana do organizacji spektakli teatralnych, przedstawień, recitalów oraz wydarzeń artystycznych. Wyposażona w profesjonalne oświetlenie sceniczne, nagłośnienie oraz przestronną scenę, która zapewnia doskonałą widoczność i akustykę. To przestrzeń, która sprzyja artystycznemu wyrazowi i tworzeniu niezapomnianych doznań dla widzów.",
                    PhotoName = "teatralna.jpg"
                },
                new HallType
                {
                    Id = 4,
                    Name = "Sala wystawowa",
                    Description = "Przestronna sala wystawowa dedykowana organizacji wystaw, targów i ekspozycji artystycznych. Zapewnia dużą powierzchnię do prezentacji dzieł sztuki, rzemiosła i innych ekspozycji. Zmodernizowane oświetlenie i możliwość aranżacji przestrzeni pozwalają na profesjonalne wyeksponowanie dzieł. Jest to przestrzeń elastyczna, doskonała do organizacji wydarzeń kulturalnych i wystaw artystycznych.",
                    PhotoName = "wystawowa.jpg"
                },
                new HallType
                {
                    Id = 5,
                    Name = "Sala ogólna",
                    Description = "Sala ogólna to wszechstronna przestrzeń, która może być wykorzystana do różnorodnych wydarzeń, takich jak konferencje, szkolenia, spotkania biznesowe, czy kameralne wydarzenia kulturalne. Elastyczne ustawienie sali oraz nowoczesne wyposażenie audiowizualne sprawiają, że jest to idealne miejsce do organizacji wydarzeń o różnym charakterze. Wysoka jakość akustyki i komfortowe warunki zapewniają dogodną atmosferę.",
                    PhotoName = "ogolna.jpg"
                },
                new HallType
                {
                    Id = 6,
                    Name = "Sala konferencyjna",
                    Description = "Sala konferencyjna dedykowana organizacji spotkań biznesowych, konferencji, prezentacji i seminariów. Wyposażona w nowoczesny sprzęt audiowizualny, systemy do wideokonferencji oraz przestronną salę, która pomieści większą liczbę uczestników. Komfortowe, ergonomiczne fotele i stół konferencyjny sprzyjają profesjonalnym spotkaniom, a elastyczne ustawienie sali pozwala dostosować ją do różnych formatów spotkań.",
                    PhotoName = "konferencyjna.jpg"
                }
            );
        }
    }
}
