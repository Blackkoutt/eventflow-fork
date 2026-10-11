using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class NewsSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<News> entityBuilder, DateTime today)
        {
            entityBuilder.HasData(
               new News
               {
                   Id = 1,
                   Title = "Finał konkursu artystycznego",
                   NewsGuid = new Guid("c771ea3f-aa24-45c9-9fde-76695d5eab1d"),
                   PublicationDate = today,
                   ShortDescription = "Z radością ogłaszamy finał naszego konkursu artystycznego, w którym uczestnicy" +
                   " rywalizowali w różnych kategoriach. Wydarzenie przyciągnęło wielu utalentowanych artystów." +
                   " Zwycięzcy otrzymali cenne nagrody i wyróżnienia.",
                   LongDescription = "Finał konkursu artystycznego, który odbył się w naszym centrum," +
                   " okazał się prawdziwym świętem sztuki. Udział wzięli artyści z różnych dziedzin: malarstwo," +
                   " rzeźba, fotografia oraz sztuki wizualne. Każdy z uczestników miał okazję zaprezentować swoje dzieła" +
                   " przed szerszą publicznością, a także przed specjalnie powołaną komisją, składającą się z profesjonalistów" +
                   " z branży artystycznej. Zwycięzcy konkursu otrzymali nagrody pieniężne oraz profesjonalne wsparcie w" +
                   " dalszym rozwoju kariery artystycznej. Wydarzenie zakończyło się uroczystą galą, która na długo pozostanie" +
                   " w pamięci uczestników i widzów.",
                   PhotoName = "konkurs_artystyczny.png"
               },
              new News
              {
                  Id = 2,
                  Title = "Relacja z koncertu zespołu Lunar Vibes",
                  NewsGuid = new Guid("94ab03cd-63d8-4516-a45b-990351680077"),
                  PublicationDate = today,
                  ShortDescription = "Zespół Lunar Vibes zachwycił publiczność podczas swojego koncertu w naszej sali" +
                  " koncertowej. Przenoszący w inny wymiar dźwięk, energetyzujący występ, który zapisał się na stałe" +
                  " w pamięci uczestników.",
                  LongDescription = "Koncert zespołu Lunar Vibes, który odbył się w naszej sali koncertowej," +
                  " przyciągnął rzeszę miłośników muzyki alternatywnej i elektronicznej. Zespół, znany z dynamicznych" +
                  " i innowacyjnych brzmień, zapewnił publiczności niezapomniane przeżycia muzyczne. Dźwięki, które" +
                  " łączyły różnorodne gatunki muzyczne, wprowadziły słuchaczy w trans, a energetyzujące występy" +
                  " artystów zapewniły fantastyczną atmosferę. W trakcie koncertu widzowie mogli usłyszeć zarówno" +
                  " starsze utwory zespołu, jak i nowości, które dopiero mają trafić na nadchodzący album. Zespół zagrał" +
                  " przed pełną salą, a po wydarzeniu odbył się kameralny after party, podczas którego fani mieli okazję" +
                  " porozmawiać z artystami. Koncert Lunar Vibes był jednym z najważniejszych wydarzeń muzycznych w tym roku.",
                  PhotoName = "koncert_lunar_vibes.png"
              },
              new News
              {
                  Id = 3,
                  Title = "Modernizacja sali koncertowej",
                  NewsGuid = new Guid("253bc760-25d9-43ef-89dc-28453c96ef9a"),
                  PublicationDate = today,
                  ShortDescription = "Nasza sala koncertowa zyskała nowy wygląd i nowoczesne wyposażenie, które pozwoli" +
                  " na organizację jeszcze lepszych wydarzeń muzycznych. Zapraszamy do odwiedzenia odnowionego wnętrza.",
                  LongDescription = "Z radością ogłaszamy zakończenie modernizacji naszej sali koncertowej. Dzięki" +
                  " inwestycjom w nowoczesne technologie, poprawiliśmy zarówno akustykę, jak i komfort naszych gości." +
                  " Wymieniono systemy nagłośnienia i oświetlenia, a także dostosowano przestrzeń do większych wydarzeń," +
                  " takich jak koncerty, festiwale muzyczne czy transmisje na żywo. Zwiększona pojemność sali oraz nowoczesne" +
                  " fotele zapewniają wygodę i komfort nawet podczas długotrwałych wydarzeń. Ponadto wprowadziliśmy innowacyjne" +
                  " rozwiązania z zakresu multimediów, dzięki czemu organizacja eventów o zróżnicowanej tematyce stała się" +
                  " jeszcze łatwiejsza. Od teraz, nasza sala koncertowa oferuje jeszcze lepsze warunki dla artystów" +
                  " i publiczności. Zapraszamy do odwiedzenia nowego, odmienionego wnętrza.",
                  PhotoName = "modernizacja sali.png"
              },
               new News
               {
                   Id = 4,
                   Title = "Zniżka 20% na zakup karnetów",
                   NewsGuid = new Guid("457bb9e7-2489-4854-952c-b92c452a7e63"),
                   PublicationDate = today,
                   ShortDescription = "Zachęcamy do zakupu karnetów na nadchodzące wydarzenia w naszym centrum." +
                   " Tylko teraz oferujemy 20% zniżki na wszystkie karnety. To doskonała okazja, by nie przegapić" +
                   " żadnego z naszych wydarzeń.",
                   LongDescription = "Przygotowaliśmy wyjątkową ofertę dla naszych stałych i nowych gości. Tylko teraz" +
                   " oferujemy 20% zniżki na wszystkie karnety w sprzedaży. Karnet upoważnia do udziału w wydarzeniach" +
                   " organizowanych w naszym centrum przez najbliższy sezon. Oferujemy szeroką gamę wydarzeń, od koncertów," +
                   " przez wystawy artystyczne, po spektakle teatralne. Zniżka dotyczy zarówno karnetów indywidualnych," +
                   " jak i grupowych. To doskonała okazja, by zagwarantować sobie dostęp do najlepszych wydarzeń w" +
                   " atrakcyjnej cenie. Oferta jest ograniczona czasowo, dlatego warto się pospieszyć i skorzystać z tej" +
                   " wyjątkowej promocji, zanim będzie za późno.",
                   PhotoName = "znizka.png"
               },
               new News
               {
                   Id = 5,
                   Title = "Noc Filmowa z Klasykami Kina",
                   NewsGuid = new Guid("57da63cb-91af-4afb-9b3b-092f72620f75"),
                   PublicationDate = today,
                   ShortDescription = "Zapraszamy na wyjątkowy wieczór z klasykami kina. Noc Filmowa to doskonała okazja," +
                   " by obejrzeć kultowe filmy na dużym ekranie w towarzystwie innych miłośników kina.",
                   LongDescription = "Noc Filmowa to wydarzenie, które przyciąga wszystkich miłośników dobrego kina." +
                   " Podczas tego specjalnego wieczoru będziemy wyświetlać klasyki światowego kina, które na zawsze" +
                   " zapisały się w historii filmografii. Filmy będą prezentowane na dużym ekranie, a publiczność będzie" +
                   " miała okazję zobaczyć je w towarzystwie innych pasjonatów kina. Zadbaliśmy o wyjątkową atmosferę," +
                   " w tym profesjonalne nagłośnienie i oświetlenie, by każda projekcja była niezapomnianym przeżyciem." +
                   " To wydarzenie jest doskonałą okazją, by spędzić czas w gronie osób, które dzielą tę samą pasję do sztuki" +
                   " filmowej. Zapraszamy do wspólnego świętowania klasyki kina.",
                   PhotoName = "noc_filmowa.png"
               },
               new News
               {
                   Id = 6,
                   Title = "Wernisaż: Nowe inspiracje",
                   NewsGuid = new Guid("8bb51e9e-1807-48b8-b48b-5eb6cfa04980"),
                   PublicationDate = today,
                   ShortDescription = "Zapraszamy na wernisaż wystawy 'Nowe inspiracje'. Artyści z różnych dziedzin sztuki" +
                   " zaprezentują swoje najnowsze prace. To doskonała okazja, by odkryć świeże spojrzenie na współczesną sztukę.",
                   LongDescription = "Wernisaż wystawy pt. 'Nowe inspiracje' to wydarzenie, które łączy w sobie różne formy" +
                   " sztuki współczesnej. Na wystawie zaprezentowane zostaną prace artystów z różnych dziedzin: malarstwo," +
                   " fotografia, rzeźba oraz multimedia. Każdy z artystów pokaże swoje najnowsze dzieła, które są wynikiem" +
                   " ich poszukiwań i eksperymentów artystycznych. Wystawa daje możliwość zobaczenia sztuki w jej najnowszej" +
                   " odsłonie i odkrycia nowych, świeżych inspiracji. Wernisaż to doskonała okazja, by spotkać się z twórcami," +
                   " porozmawiać o ich pracy i zbliżyć się do współczesnej sztuki. Wydarzenie odbędzie się w naszej przestronnej" +
                   " galerii, która doskonale nadaje się do prezentacji tego typu wystaw.",
                   PhotoName = "wernisaz.png"
               }
            );
        }
    }
}
