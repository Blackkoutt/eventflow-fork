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
                    CategoryId = 1,
                    HallId = 6,
                    EventGuid = new Guid("bd9cf606-260b-4611-bb6f-3cde4298edc7"),
                    PhotoName = "koncert_mystic_waves.png",
                    LongDescription = "Koncert Mystic Waves to wyjątkowe wydarzenie muzyczne," +
                    " które przeniesie Cię w świat atmosferycznych dźwięków." +
                    " Zespół wystąpi z premierowym materiałem, łączącym elementy muzyki elektronicznej" +
                    " i ambientowej. Koncert odbędzie się w klimatycznej sali," +
                    " z wykorzystaniem nowoczesnej technologii dźwiękowej i wizualnej," +
                    " co zapewni niezapomniane przeżycia. Muzycy znani z licznych występów" +
                    " na scenach festiwalowych zaprezentują swoją najnowszą produkcję, która" +
                    " z pewnością oczaruje fanów muzyki eksperymentalnej."
                },
                new Event
                {
                    Id = 2,
                    Name = "Cień Przeszłości",
                    ShortDescription = "Dramatyczna opowieść o sekretach przeszłości, które powracają, by zmienić teraźniejszość.",
                    StartDate = today.AddMonths(1).AddDays(2),
                    EndDate = today.AddMonths(1).AddDays(2).AddHours(3),
                    CategoryId = 3,
                    HallId = 7,
                    EventGuid = new Guid("30f24949-51df-4274-a804-544ee7e2e1d2"),
                    PhotoName = "cien_przeszlosci.png",
                    LongDescription = "Spektakl 'Cień Przeszłość' to wciągająca produkcja teatralna," +
                    " która porusza tematykę przemijania i refleksji nad przeszłością." +
                    " Historia opowiada o dwóch postaciach, które spotykają się po latach," +
                    " by skonfrontować swoje wspomnienia i tajemnice. Reżyseria, mistrzowskie" +
                    " aktorstwo oraz emocjonalna głębia tej sztuki sprawiają, że jest to wydarzenie," +
                    " które zostanie z widzami na długo. Spektakl jest pełen symboliki i zaskakujących" +
                    " zwrotów akcji, które skłonią do przemyśleń o upływającym czasie."
                },
                new Event
                {
                    Id = 3,
                    Name = "Królestwo planety małp",
                    ShortDescription = "Kolejna część kultowej serii sci-fi. Świat, w którym dominują małpy, a ludzie walczą o przetrwanie.",
                    StartDate = today.AddMonths(1).AddDays(3),
                    EndDate = today.AddMonths(1).AddDays(3).AddHours(2),
                    CategoryId = 2,
                    HallId = 5,
                    EventGuid = new Guid("63b9ba64-48dd-4997-95c9-dbfa2389f066"),
                    PhotoName = "krolestwo_planety_malp.png",
                    LongDescription = "Film 'Królestwo planety małp' to spektakularna produkcja" +
                    " science-fiction, która kontynuuje historię walczących o przetrwanie małp." +
                    " Akcja filmu toczy się w post-apokaliptycznym świecie, w którym małpy zaczynają" +
                    " dominować nad ludźmi. W filmie zobaczymy dynamiczne sceny bitewne," +
                    " dramatyczne decyzje bohaterów i wciągającą fabułę, która porusza kwestie" +
                    " moralności i przyszłości ludzkości. Wysoka jakość efektów specjalnych oraz" +
                    " rewelacyjna gra aktorska sprawiają, że jest to jeden z najważniejszych filmów" +
                    " tego roku."
                },
                new Event
                {
                    Id = 4,
                    Name = "Nowe inspiracje",
                    ShortDescription = "Wystawa prezentująca prace nowoczesnych artystów, pełna świeżych perspektyw i innowacji.",
                    StartDate = today.AddMonths(1).AddDays(4),
                    EndDate = today.AddMonths(1).AddDays(4).AddHours(3),
                    CategoryId = 4,
                    HallId = 8,
                    EventGuid = new Guid("5e01e785-8f63-4f8c-af89-25ee89f22b83"),
                    PhotoName = "nowe_inspiracje.png",
                    LongDescription = "Wystawa 'Nowe inspiracje' to wyjątkowa ekspozycja" +
                    " sztuki współczesnej, która gromadzi prace młodych artystów z całego" +
                    " świata. Wystawa skupia się na nowych nurtach artystycznych, które łączą" +
                    " tradycję z nowoczesnością. Można tu zobaczyć prace w różnych mediach," +
                    " od malarstwa po instalacje interaktywne. Artyści eksperymentują z formą" +
                    " i materiałami, zapraszając widzów do interakcji z dziełami. Jest to przestrzeń," +
                    " która otwiera umysł na nowe pomysły i inspiracje, zachęcając do refleksji" +
                    " nad współczesnym światem sztuki."
                },
                new Event
                {
                    Id = 5,
                    Name = "Koncert: New Era",
                    ShortDescription = "Energetyczny koncert zespołu New Era – mieszanka rocka, elektroniki i alternatywnych brzmień.",
                    StartDate = today.AddMonths(2).AddDays(1),
                    EndDate = today.AddMonths(2).AddDays(1).AddHours(1),
                    CategoryId = 1,
                    HallId = 10,
                    EventGuid = new Guid("7f940a53-f3a2-46d8-8d39-b28e076d40aa"),
                    PhotoName = "new_era.png",
                    LongDescription = "Koncert 'New Era' to wyjątkowe wydarzenie muzyczne," +
                    " które łączy najnowsze trendy w muzyce elektronicznej z klasycznymi brzmieniami." +
                    " Zespół zaprezentuje utwory z debiutanckiego albumu, który zdobył uznanie wśród" +
                    " krytyków muzycznych. Występ będzie pełen energetycznych melodii i przejmujących" +
                    " dźwięków, które z pewnością poruszą serca słuchaczy. Nowoczesne technologie" +
                    " audio-wizualne, wykorzystywane podczas koncertu, sprawią, że każdy utwór nabierze" +
                    " nowego wymiaru. Warto być częścią tego muzycznego doświadczenia!"
                },
                new Event
                {
                    Id = 6,
                    Name = "Gladiator",
                    ShortDescription = "Legendarna opowieść o honorze i zemście w starożytnym Rzymie.",
                    StartDate = today.AddMonths(2).AddDays(3),
                    EndDate = today.AddMonths(2).AddDays(3).AddHours(2),
                    CategoryId = 2,
                    HallId = 9,
                    EventGuid = new Guid("bc7b2f85-b498-4186-ad5c-5cf25d938499"),
                    PhotoName = "gladiator.png",
                    LongDescription = "Film 'Gladiator' to epicka opowieść o wojowniku," +
                    " który staje do walki nie tylko o swoje życie, ale także o wolność" +
                    " i sprawiedliwość. Akcja rozgrywa się w starożytnym Rzymie, gdzie główny" +
                    " bohater, Maximus, zostaje zdradzony przez cesarza, a jego rodzina zostaje" +
                    " zamordowana. Maximus trafia do areny, gdzie walczy o przetrwanie i zemstę." +
                    " Film pełen jest emocjonujących walk, dramatycznych zwrotów akcji i wciągającej" +
                    " fabuły, która porusza tematy honoru, lojalności i zemsty."
                },
                new Event
                {
                    Id = 7,
                    Name = "Nowa sztuka",
                    ShortDescription = "Wystawa prezentująca najnowsze prace artystów eksperymentalnych i awangardowych.",
                    StartDate = today.AddMonths(2).AddDays(4),
                    EndDate = today.AddMonths(2).AddDays(4).AddHours(3),
                    CategoryId = 4,
                    HallId = 11,
                    EventGuid = new Guid("775b339d-4524-4829-bd42-4da8f88cf3f1"),
                    LongDescription = "Wystawa 'Nowa sztuka' to prezentacja młodych artystów," +
                    " którzy swoją twórczość ukierunkowali na przekroczenie granic tradycyjnej" +
                    " sztuki. Na wystawie można zobaczyć prace w różnorodnych technikach:" +
                    " od klasycznych obrazów po awangardowe instalacje. Artyści bawią się konwencjami," +
                    " angażując widza w interaktywną formę prezentacji. Wystawa jest" +
                    " zaproszeniem do refleksji nad tym, czym współczesna sztuka może być " +
                    "i jakie emocje potrafi wywołać. To wydarzenie, które pokazuje, jak sztuka może" +
                    " zmieniać nasze postrzeganie świata."
                },
                new Event
                {
                    Id = 8,
                    Name = "Cień Nad Miastem",
                    ShortDescription = "Mroczny thriller teatralny o tajemniczych wydarzeniach, które wstrząsają miastem po zmroku.",
                    StartDate = today.AddMonths(2).AddDays(15),
                    EndDate = today.AddMonths(2).AddDays(15).AddHours(2).AddMinutes(30),
                    CategoryId = 3,
                    HallId = 16,
                    EventGuid = new Guid("08050835-f7b3-4395-9679-b7bc915c84a6"),
                    PhotoName = "cien_nad_miastem.png",
                    LongDescription = "Cień Nad Miastem to mroczny thriller teatralny, który przenosi widza w" +
                    " świat pełen tajemniczych wydarzeń, które zaczynają wstrząsać miastem po zmroku." +
                    " Główna fabuła opowiada historię, w której pozornie spokojne miasto staje się areną niebezpiecznych" +
                    " wydarzeń, których rozwiązanie wciąga nie tylko bohaterów, ale również publiczność. W spektaklu dominują" +
                    " napięcie i atmosfera niepokoju, które potęgują świetnie dobrane efekty wizualne oraz świetne aktorstwo." +
                    " Każda scena jest przemyślana, a dramaturgia składa się z zagadek i zwrotów akcji, które trzymają widza" +
                    " w ciągłym napięciu. To przedstawienie nie tylko o mrocznych wydarzeniach, ale także o ludzkich emocjach," +
                    " które rodzą się w sytuacjach kryzysowych, zmieniając życie bohaterów na zawsze. W trakcie spektaklu" +
                    " widzowie będą mieli okazję odkryć tajemnice miasta i skonfrontować się z pytaniem, jak cienka jest" +
                    " granica między rzeczywistością a tym, co wytworzyła wyobraźnia."
                },
                new Event
                {
                    Id = 9,
                    Name = "Jazzowe Brzmienia",
                    ShortDescription = "Magiczny czas z jazzem na żywo – koncert znanych artystów jazzowych, którzy przeniosą Cię w świat wyjątkowych dźwięków.",
                    StartDate = today.AddDays(-1).AddHours(1),
                    EndDate = today.AddDays(-1).AddHours(3),
                    CategoryId = 1,
                    HallId = 17,
                    EventGuid = new Guid("a4f24bff-10c1-4763-b410-ba0e6e7bb3d3"),
                    PhotoName = "jazz.png",
                    LongDescription = "Jazzowe Brzmienia to wyjątkowy koncert, który przenosi uczestników w świat żywego jazzu." +
                    " Podczas wydarzenia wystąpią znani artyści jazzowi, którzy zabiorą widzów w podróż po różnorodnych stylach tego gatunku." +
                    " Atmosfera pełna emocji, improwizacji i magicznych dźwięków sprawia, że każda chwila staje się niezapomnianym doświadczeniem." +
                    " Koncert jest idealną okazją, by poczuć autentyczną magię jazzu w wyjątkowym wykonaniu na żywo, a publiczność wciągnie" +
                    " się w niepowtarzalny klimat muzycznej ekspresji."
                },
                new Event
                {
                    Id = 10,
                    Name = "Interstellar",
                    ShortDescription = "Epicka podróż kosmiczna, która bada granice ludzkiej wytrzymałości i miłości.",
                    StartDate = today.AddDays(-2).AddHours(2),
                    EndDate = today.AddDays(-2).AddHours(4),
                    CategoryId = 2,
                    HallId = 18,
                    EventGuid = new Guid("f9b817c5-e386-4107-9ed3-49b7ce38c519"),
                    PhotoName = "interstellar.jpg",
                    LongDescription = "Interstellar to epicka podróż w głąb kosmosu, która bada granice ludzkiej wytrzymałości, miłości i poświęcenia." +
                    " Grupa astronautów, wyruszając na misję w poszukiwaniu nowego domu dla ludzkości, zmierzy się z niewyobrażalnymi wyzwaniami." +
                    " Film porusza tematykę czasu, przestrzeni i ofiary, ukazując niesamowite krajobrazy kosmiczne oraz ludzką determinację w walce" +
                    " o przetrwanie. Każdy moment to prawdziwa uczta dla zmysłów, a historia miłości i poświęcenia łączy się z naukową fascynacją," +
                    " tworząc niezapomniane doświadczenie filmowe."
                },
                new Event
                {
                    Id = 11,
                    Name = "Zatrzymać Przeznaczenie",
                    ShortDescription = "Spektakl o postaci, która staje w obliczu nieuchronnego przeznaczenia.",
                    StartDate = today.AddDays(-3).AddHours(3),
                    EndDate = today.AddDays(-3).AddHours(5),
                    CategoryId = 3,
                    HallId = 19,
                    EventGuid = new Guid("ef97fdf1-9b04-47aa-bc42-1ff4f7fd5a07"),
                    PhotoName = "zatrzymac_przeznaczenie.jpg",
                    LongDescription = "Spektakl 'Zatrzymać Przeznaczenie' to głęboka opowieść o postaci, która staje przed obliczem nieuchronnego przeznaczenia." +
                    " Bohaterowie zmagają się z wyborem, który może zmienić bieg wydarzeń, a ich działania prowadzą do nieodwracalnych konsekwencji." +
                    " Spektakl skłania do refleksji nad wolną wolą, nieuchronnością losu i odpowiedzialnością za własne wybory. Z pomocą wciągającej dramaturgii" +
                    " oraz poruszających dialogów, spektakl ukazuje, jak delikatna jest granica między tym, co możemy kontrolować, a tym, co jest zapisane w naszych losach."
                },
                new Event
                {
                    Id = 12,
                    Name = "Nowe Perspektywy",
                    ShortDescription = "Wystawa sztuki współczesnej, która zaprasza do odkrywania świata z nowych punktów widzenia.",
                    StartDate = today.AddDays(-4).AddHours(4),
                    EndDate = today.AddDays(-4).AddHours(6),
                    CategoryId = 4,
                    HallId = 20,
                    EventGuid = new Guid("3c229abc-c4e2-41e9-9aad-487cd733f8a5"),
                    PhotoName = "nowe_perspektywy.jpg",
                    LongDescription = "Wystawa 'Nowe Perspektywy' zaprasza do odkrywania sztuki współczesnej z zupełnie nowych punktów widzenia." +
                    " Artyści wykorzystują różnorodne techniki, od tradycyjnych obrazów po innowacyjne instalacje, które angażują widza w interaktywną" +
                    " formę prezentacji. Każda praca na wystawie to zaproszenie do zastanowienia się nad tym, jak sztuka może zmieniać nasze postrzeganie" +
                    " rzeczywistości. To także próba przekroczenia granic tradycyjnych konwencji, poszukiwania nowych środków wyrazu i głębokiej refleksji" +
                    " nad naturą współczesnej sztuki."
                }
            );
        }
    }
}
