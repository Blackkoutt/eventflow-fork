using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class EquipmentSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Equipment> entityBuilder)
        {
            entityBuilder.HasData(
                new Equipment
                {
                    Id = 1,
                    Name = "Projektor multimedialny",
                    Description = "Nowoczesny projektor, idealny do prezentacji multimedialnych, z wysoką rozdzielczością i możliwością podłączenia różnych źródeł sygnału."
                },
                new Equipment
                {
                    Id = 2,
                    Name = "Oświetlenie",
                    Description = "Wysokiej klasy oświetlenie, umożliwiające regulację jasności i kolorów, idealne do sal konferencyjnych i eventów."
                },
                new Equipment
                {
                    Id = 3,
                    Name = "Nagłośnienie kinowe",
                    Description = "Głośniki przeznaczone do odtwarzania filmów, zapewniające doskonałą jakość dźwięku przestrzennego w dużych przestrzeniach."
                },
                new Equipment
                {
                    Id = 4,
                    Name = "Nagłośnienie koncertowe",
                    Description = "Głośniki przeznaczone do koncertów, oferujące mocny dźwięk i wyrazistość w każdych warunkach, z możliwością regulacji."
                },
                new Equipment
                {
                    Id = 5,
                    Name = "Mikrofony bezprzewodowe",
                    Description = "Profesjonalne mikrofony bezprzewodowe, zapewniające czysty dźwięk i niezawodność w ruchu, idealne do konferencji i wystąpień."
                },
                new Equipment
                {
                    Id = 6,
                    Name = "Tablica interaktywna",
                    Description = "Tablica interaktywna do rysowania i prezentowania treści w sposób dynamiczny."
                },
                new Equipment
                {
                    Id = 7,
                    Name = "Klimatyzacja",
                    Description = "System klimatyzacji zapewniający komfortową temperaturę w sali podczas wydarzeń."
                },
                new Equipment
                {
                    Id = 8,
                    Name = "Stół konferencyjny",
                    Description = "Duży stół konferencyjny, idealny do spotkań biznesowych i prezentacji."
                },
                new Equipment
                {
                    Id = 9,
                    Name = "Kurtyna",
                    Description = "Kurtyna teatralna służąca do oddzielania sceny od widowni lub zmiany scenerii w trakcie przedstawienia."
                },
                new Equipment
                {
                    Id = 10,
                    Name = "Tablice informacyjne",
                    Description = "Tablice służące do umieszczania informacji o eksponatach."
                },
                new Equipment
                {
                    Id = 11,
                    Name = "Stojaki",
                    Description = "Stojaki umożliwiają prezentowanie dzieł sztuki, takich jak obrazy, grafiki czy rzeźby."
                }

           );
        }
    }
}
