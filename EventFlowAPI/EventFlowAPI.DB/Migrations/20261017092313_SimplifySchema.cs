using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventFlowAPI.DB.Migrations
{
    /// <inheritdoc />
    public partial class SimplifySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventDetails_Id",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_EventPass_PaymentType_PaymentTypeId",
                table: "EventPass");

            migrationBuilder.DropForeignKey(
                name: "FK_Festival_FestivalDetails_Id",
                table: "Festival");

            migrationBuilder.DropForeignKey(
                name: "FK_HallRent_PaymentType_PaymentTypeId",
                table: "HallRent");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_PaymentType_PaymentTypeId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_TicketPDF_TicketPDFId",
                table: "Reservation");

            migrationBuilder.DropTable(
                name: "EventDetails");

            migrationBuilder.DropTable(
                name: "Festival_MediaPatron");

            migrationBuilder.DropTable(
                name: "Festival_Organizer");

            migrationBuilder.DropTable(
                name: "Festival_Sponsor");

            migrationBuilder.DropTable(
                name: "FestivalDetails");

            migrationBuilder.DropTable(
                name: "HallDetails");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropTable(
                name: "Reservation_TicketJPG");

            migrationBuilder.DropTable(
                name: "TicketPDF");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "MediaPatron");

            migrationBuilder.DropTable(
                name: "Organizer");

            migrationBuilder.DropTable(
                name: "Sponsor");

            migrationBuilder.DropTable(
                name: "TicketJPG");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_PaymentTypeId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_TicketPDFId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_HallRent_PaymentTypeId",
                table: "HallRent");

            migrationBuilder.DropIndex(
                name: "IX_EventPass_PaymentTypeId",
                table: "EventPass");

            migrationBuilder.DropColumn(
                name: "IsSoftUpdated",
                table: "TicketType");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "TicketType");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "TicketType");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "IsSoftUpdated",
                table: "SeatType");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "SeatType");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "SeatType");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "Seat");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Seat");

            migrationBuilder.DropColumn(
                name: "IsFestivalReservation",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "TicketPDFId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "IsSoftUpdated",
                table: "HallType");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "HallType");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "HallType");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "HallRent");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "HallRent");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "HallRent");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "HallRent");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Festival");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "Festival");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Festival");

            migrationBuilder.DropColumn(
                name: "IsSoftUpdated",
                table: "EventPassType");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "EventPassType");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "EventPassType");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "EventPass");

            migrationBuilder.DropColumn(
                name: "PreviousEndDate",
                table: "EventPass");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "EventPass");

            migrationBuilder.DropColumn(
                name: "IsSoftUpdated",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "IsSoftUpdated",
                table: "AdditionalServices");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "AdditionalServices");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "AdditionalServices");

            migrationBuilder.RenameColumn(
                name: "PaymentTypeId",
                table: "Reservation",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "PaymentTypeId",
                table: "HallRent",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "PaymentTypeId",
                table: "EventPass",
                newName: "PaymentType");

            migrationBuilder.CreateSequence<int>(
                name: "EventIdSequence",
                startValue: 13L);

            migrationBuilder.CreateSequence<int>(
                name: "FestivalIdSequence",
                startValue: 4L);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FlatNumber",
                table: "Users",
                type: "NUMERIC(3,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HouseNumber",
                table: "Users",
                type: "NUMERIC(3,0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Users",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketJPGFileNames",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "TicketPDFFileName",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Partner",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Partner",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Partner",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Kind",
                table: "Partner",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxNumberOfSeats",
                table: "Hall",
                type: "NUMERIC(3,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxNumberOfSeatsColumns",
                table: "Hall",
                type: "NUMERIC(2,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxNumberOfSeatsRows",
                table: "Hall",
                type: "NUMERIC(2,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NumberOfSeats",
                table: "Hall",
                type: "NUMERIC(3,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StageLength",
                table: "Hall",
                type: "NUMERIC(4,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StageWidth",
                table: "Hall",
                type: "NUMERIC(4,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalLength",
                table: "Hall",
                type: "NUMERIC(4,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWidth",
                table: "Hall",
                type: "NUMERIC(4,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Festival",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [FestivalIdSequence]",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Festival",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [EventIdSequence]",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Event",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Festival_Partner",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    PartnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival_Partner", x => new { x.FestivalId, x.PartnerId });
                    table.ForeignKey(
                        name: "FK_Festival_Partner_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Festival_Partner_Partner_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1,
                column: "LongDescription",
                value: "Koncert Mystic Waves to wyjątkowe wydarzenie muzyczne, które przeniesie Cię w świat atmosferycznych dźwięków. Zespół wystąpi z premierowym materiałem, łączącym elementy muzyki elektronicznej i ambientowej. Koncert odbędzie się w klimatycznej sali, z wykorzystaniem nowoczesnej technologii dźwiękowej i wizualnej, co zapewni niezapomniane przeżycia. Muzycy znani z licznych występów na scenach festiwalowych zaprezentują swoją najnowszą produkcję, która z pewnością oczaruje fanów muzyki eksperymentalnej.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2,
                column: "LongDescription",
                value: "Spektakl 'Cień Przeszłość' to wciągająca produkcja teatralna, która porusza tematykę przemijania i refleksji nad przeszłością. Historia opowiada o dwóch postaciach, które spotykają się po latach, by skonfrontować swoje wspomnienia i tajemnice. Reżyseria, mistrzowskie aktorstwo oraz emocjonalna głębia tej sztuki sprawiają, że jest to wydarzenie, które zostanie z widzami na długo. Spektakl jest pełen symboliki i zaskakujących zwrotów akcji, które skłonią do przemyśleń o upływającym czasie.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3,
                column: "LongDescription",
                value: "Film 'Królestwo planety małp' to spektakularna produkcja science-fiction, która kontynuuje historię walczących o przetrwanie małp. Akcja filmu toczy się w post-apokaliptycznym świecie, w którym małpy zaczynają dominować nad ludźmi. W filmie zobaczymy dynamiczne sceny bitewne, dramatyczne decyzje bohaterów i wciągającą fabułę, która porusza kwestie moralności i przyszłości ludzkości. Wysoka jakość efektów specjalnych oraz rewelacyjna gra aktorska sprawiają, że jest to jeden z najważniejszych filmów tego roku.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4,
                column: "LongDescription",
                value: "Wystawa 'Nowe inspiracje' to wyjątkowa ekspozycja sztuki współczesnej, która gromadzi prace młodych artystów z całego świata. Wystawa skupia się na nowych nurtach artystycznych, które łączą tradycję z nowoczesnością. Można tu zobaczyć prace w różnych mediach, od malarstwa po instalacje interaktywne. Artyści eksperymentują z formą i materiałami, zapraszając widzów do interakcji z dziełami. Jest to przestrzeń, która otwiera umysł na nowe pomysły i inspiracje, zachęcając do refleksji nad współczesnym światem sztuki.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5,
                column: "LongDescription",
                value: "Koncert 'New Era' to wyjątkowe wydarzenie muzyczne, które łączy najnowsze trendy w muzyce elektronicznej z klasycznymi brzmieniami. Zespół zaprezentuje utwory z debiutanckiego albumu, który zdobył uznanie wśród krytyków muzycznych. Występ będzie pełen energetycznych melodii i przejmujących dźwięków, które z pewnością poruszą serca słuchaczy. Nowoczesne technologie audio-wizualne, wykorzystywane podczas koncertu, sprawią, że każdy utwór nabierze nowego wymiaru. Warto być częścią tego muzycznego doświadczenia!");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6,
                column: "LongDescription",
                value: "Film 'Gladiator' to epicka opowieść o wojowniku, który staje do walki nie tylko o swoje życie, ale także o wolność i sprawiedliwość. Akcja rozgrywa się w starożytnym Rzymie, gdzie główny bohater, Maximus, zostaje zdradzony przez cesarza, a jego rodzina zostaje zamordowana. Maximus trafia do areny, gdzie walczy o przetrwanie i zemstę. Film pełen jest emocjonujących walk, dramatycznych zwrotów akcji i wciągającej fabuły, która porusza tematy honoru, lojalności i zemsty.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 7,
                column: "LongDescription",
                value: "Wystawa 'Nowa sztuka' to prezentacja młodych artystów, którzy swoją twórczość ukierunkowali na przekroczenie granic tradycyjnej sztuki. Na wystawie można zobaczyć prace w różnorodnych technikach: od klasycznych obrazów po awangardowe instalacje. Artyści bawią się konwencjami, angażując widza w interaktywną formę prezentacji. Wystawa jest zaproszeniem do refleksji nad tym, czym współczesna sztuka może być i jakie emocje potrafi wywołać. To wydarzenie, które pokazuje, jak sztuka może zmieniać nasze postrzeganie świata.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 8,
                column: "LongDescription",
                value: "Cień Nad Miastem to mroczny thriller teatralny, który przenosi widza w świat pełen tajemniczych wydarzeń, które zaczynają wstrząsać miastem po zmroku. Główna fabuła opowiada historię, w której pozornie spokojne miasto staje się areną niebezpiecznych wydarzeń, których rozwiązanie wciąga nie tylko bohaterów, ale również publiczność. W spektaklu dominują napięcie i atmosfera niepokoju, które potęgują świetnie dobrane efekty wizualne oraz świetne aktorstwo. Każda scena jest przemyślana, a dramaturgia składa się z zagadek i zwrotów akcji, które trzymają widza w ciągłym napięciu. To przedstawienie nie tylko o mrocznych wydarzeniach, ale także o ludzkich emocjach, które rodzą się w sytuacjach kryzysowych, zmieniając życie bohaterów na zawsze. W trakcie spektaklu widzowie będą mieli okazję odkryć tajemnice miasta i skonfrontować się z pytaniem, jak cienka jest granica między rzeczywistością a tym, co wytworzyła wyobraźnia.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 9,
                column: "LongDescription",
                value: "Jazzowe Brzmienia to wyjątkowy koncert, który przenosi uczestników w świat żywego jazzu. Podczas wydarzenia wystąpią znani artyści jazzowi, którzy zabiorą widzów w podróż po różnorodnych stylach tego gatunku. Atmosfera pełna emocji, improwizacji i magicznych dźwięków sprawia, że każda chwila staje się niezapomnianym doświadczeniem. Koncert jest idealną okazją, by poczuć autentyczną magię jazzu w wyjątkowym wykonaniu na żywo, a publiczność wciągnie się w niepowtarzalny klimat muzycznej ekspresji.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 10,
                column: "LongDescription",
                value: "Interstellar to epicka podróż w głąb kosmosu, która bada granice ludzkiej wytrzymałości, miłości i poświęcenia. Grupa astronautów, wyruszając na misję w poszukiwaniu nowego domu dla ludzkości, zmierzy się z niewyobrażalnymi wyzwaniami. Film porusza tematykę czasu, przestrzeni i ofiary, ukazując niesamowite krajobrazy kosmiczne oraz ludzką determinację w walce o przetrwanie. Każdy moment to prawdziwa uczta dla zmysłów, a historia miłości i poświęcenia łączy się z naukową fascynacją, tworząc niezapomniane doświadczenie filmowe.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 11,
                column: "LongDescription",
                value: "Spektakl 'Zatrzymać Przeznaczenie' to głęboka opowieść o postaci, która staje przed obliczem nieuchronnego przeznaczenia. Bohaterowie zmagają się z wyborem, który może zmienić bieg wydarzeń, a ich działania prowadzą do nieodwracalnych konsekwencji. Spektakl skłania do refleksji nad wolną wolą, nieuchronnością losu i odpowiedzialnością za własne wybory. Z pomocą wciągającej dramaturgii oraz poruszających dialogów, spektakl ukazuje, jak delikatna jest granica między tym, co możemy kontrolować, a tym, co jest zapisane w naszych losach.");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 12,
                column: "LongDescription",
                value: "Wystawa 'Nowe Perspektywy' zaprasza do odkrywania sztuki współczesnej z zupełnie nowych punktów widzenia. Artyści wykorzystują różnorodne techniki, od tradycyjnych obrazów po innowacyjne instalacje, które angażują widza w interaktywną formę prezentacji. Każda praca na wystawie to zaproszenie do zastanowienia się nad tym, jak sztuka może zmieniać nasze postrzeganie rzeczywistości. To także próba przekroczenia granic tradycyjnych konwencji, poszukiwania nowych środków wyrazu i głębokiej refleksji nad naturą współczesnej sztuki.");

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 1,
                column: "LongDescription",
                value: "Festiwal Muzyki Hip-Hop to coroczne wydarzenie dedykowane nowoczesnym brzmieniom i eksperymentalnym kompozycjom. Na scenie pojawiają się zarówno uznani artyści, jak i młode talenty, prezentując innowacyjne podejście do muzyki. W programie znajdują się koncerty, warsztaty, spotkania z twórcami oraz panele dyskusyjne dotyczące przyszłości muzyki. Festiwal przyciąga miłośników dźwięków elektronicznych, jazzowych improwizacji, minimalistycznych kompozycji i nowych form ekspresji dźwiękowej. To wyjątkowa okazja, by odkryć najnowsze trendy muzyczne i poszerzyć swoje horyzonty artystyczne.");

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 2,
                column: "LongDescription",
                value: "Festiwal Filmowy to wyjątkowe święto kina, które gromadzi pasjonatów filmów z różnych zakątków świata. W repertuarze znajdują się zarówno filmy fabularne, dokumentalne, jak i krótkometrażowe, reprezentujące różne nurty i stylistyki. Organizowane są spotkania z reżyserami, aktorami oraz warsztaty dla młodych filmowców. Każda edycja festiwalu poświęcona jest określonemu tematowi przewodniemu, który inspiruje twórców do dyskusji nad istotnymi kwestiami społecznymi, kulturowymi i artystycznymi. To doskonała okazja, by zobaczyć premiery, odkryć nowe talenty i zanurzyć się w magiczny świat kinematografii.");

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 3,
                column: "LongDescription",
                value: "Festiwal Sztuki to wydarzenie celebrujące kreatywność i nieograniczoną ekspresję artystyczną. W ramach festiwalu prezentowane są prace artystów, którzy eksperymentują z formą, kolorem i strukturą, tworząc unikalne dzieła sztuki nowoczesnej. Oprócz wystaw odbywają się warsztaty, podczas których uczestnicy mogą spróbować swoich sił w malarstwie, rzeźbie czy instalacjach multimedialnych. Festiwal to także przestrzeń do rozmów o współczesnych trendach w sztuce, spotkania z artystami oraz wykłady ekspertów. To niepowtarzalna okazja, by odkryć nowe inspiracje i spojrzeć na sztukę z innej perspektywy.");

            migrationBuilder.InsertData(
                table: "Festival_Partner",
                columns: new[] { "FestivalId", "PartnerId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 4 },
                    { 1, 5 },
                    { 2, 1 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 2 },
                    { 3, 3 },
                    { 3, 5 }
                });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 255m, 15m, 15m, 100m, 4.4m, 7m, 22m, 14m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 150m, 10m, 15m, 150m, null, null, 22m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 60m, 10m, 6m, 60m, 4.4m, 5.6m, 11m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 100m, 10m, 10m, 100m, 6m, 7.2m, 17m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 255m, 15m, 15m, 100m, 4.4m, 7m, 22m, 14m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 150m, 10m, 15m, 150m, null, null, 22m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 60m, 10m, 6m, 60m, 4.4m, 5.6m, 11m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 100m, 10m, 10m, 100m, 6m, 7.2m, 17m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 255m, 15m, 15m, 100m, 4.4m, 7m, 22m, 14m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 150m, 10m, 15m, 150m, null, null, 22m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 100m, 10m, 10m, 100m, 6m, 7.2m, 17m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 255m, 15m, 15m, 100m, 4.4m, 7m, 22m, 14m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 150m, 10m, 15m, 150m, null, null, 22m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 60m, 10m, 6m, 60m, 4.4m, 5.6m, 11m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 100m, 10m, 10m, 100m, 6m, 7.2m, 17m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 60m, 10m, 6m, 60m, 4.4m, 5.6m, 11m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 150m, 10m, 15m, 150m, null, null, 22m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 255m, 15m, 15m, 100m, 4.4m, 7m, 22m, 14m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 60m, 10m, 6m, 60m, 4.4m, 5.6m, 11m, 9m });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalLength", "TotalWidth" },
                values: new object[] { 100m, 10m, 10m, 100m, 6m, 7.2m, 17m, 9m });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeleteDate", "IsDeleted", "Kind", "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { null, false, "MediaPatron", "Gazeta Nowoczesna", new Guid("ae371ef5-1408-4a99-9df5-c02da3894a64"), "gazeta_nowoczesna.png" });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeleteDate", "IsDeleted", "Kind", "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { null, false, "MediaPatron", "Nowy świat TV", new Guid("1888ac98-9879-49d8-b5a0-a9bdedf65b79"), "nowy_swiat.png" });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeleteDate", "IsDeleted", "Kind", "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { null, false, "MediaPatron", "Tygodnik Nowiny", new Guid("21721629-b1aa-4fd0-8fce-689d78d6c326"), "tygodnik_nowiny.png" });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeleteDate", "IsDeleted", "Kind", "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { null, false, "Organizer", "EventFlow", new Guid("4656853b-fbf1-4407-8a15-9f60fe367343"), "eventflow.png" });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DeleteDate", "IsDeleted", "Kind", "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { null, false, "Organizer", "Snowflake", new Guid("175c000d-26c4-4519-b49d-c9d1d7935699"), "snowflake.png" });

            migrationBuilder.InsertData(
                table: "Partner",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "Kind", "Name", "PartnerGuid", "PhotoName" },
                values: new object[,]
                {
                    { 6, null, false, "Organizer", "Aura", new Guid("d044fdef-73a3-4f97-962f-d85e36760f4e"), "aura.png" },
                    { 7, null, false, "Sponsor", "Basel", new Guid("423d7369-b70c-4471-97a8-968d001718a7"), "basel.png" },
                    { 8, null, false, "Sponsor", "Vision", new Guid("6ba10301-43f6-4207-835c-15f03d7904c7"), "vision.png" },
                    { 9, null, false, "Sponsor", "Waveless", new Guid("ca9f16d2-cfa6-47dc-9aff-a0821ce6bf79"), "waveless.png" },
                    { 10, null, false, "Partner", "Basel", new Guid("8e0ab6f0-ebbd-4d96-ae19-574b276e985c"), "basel.png" },
                    { 11, null, false, "Partner", "Aura", new Guid("fa1fefc3-3a92-4162-a5cc-30aafea11323"), "aura.png" },
                    { 12, null, false, "Partner", "Vision", new Guid("bb243a20-bf87-4534-b9b2-a994004f00c4"), "vision.png" },
                    { 13, null, false, "Partner", "Snowflake", new Guid("2a1dbe6b-42f3-4fec-ab63-dffba70184b7"), "snowflake.png" },
                    { 14, null, false, "Partner", "Waveless", new Guid("20f68122-1bdf-4881-8ce4-f808701648a5"), "waveless.png" }
                });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "TicketJPGFileNames", "TicketPDFFileName" },
                values: new object[] { "[\"eventflow_bilet_test_53538b58-f885-4f4a-b675-a4aa4063ccf3.jpg\"]", "eventflow_bilet_test_53538b58-f885-4f4a-b675-a4aa4063ccf3.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "TicketJPGFileNames", "TicketPDFFileName" },
                values: new object[] { "[\"eventflow_bilet_test_ed8b9230-223b-4609-8d13-aa6017edad09.jpg\"]", "eventflow_bilet_test_ed8b9230-223b-4609-8d13-aa6017edad09.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "TicketJPGFileNames", "TicketPDFFileName" },
                values: new object[] { "[\"eventflow_bilet_test_f9a076c4-3475-4a28-a60c-6e0e3c03731a.jpg\"]", "eventflow_bilet_test_f9a076c4-3475-4a28-a60c-6e0e3c03731a.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "TicketJPGFileNames", "TicketPDFFileName" },
                values: new object[] { "[\"eventflow_bilet_test_de1d6773-f027-4888-996a-0296e5c52708.jpg\"]", "eventflow_bilet_test_de1d6773-f027-4888-996a-0296e5c52708.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "TicketJPGFileNames", "TicketPDFFileName" },
                values: new object[] { "[\"eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd_1.jpg\",\"eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd_2.jpg\"]", "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "TicketJPGFileNames", "TicketPDFFileName" },
                values: new object[] { "[\"eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd_1.jpg\",\"eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd_2.jpg\"]", "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "TicketJPGFileNames", "TicketPDFFileName" },
                values: new object[] { "[\"eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6_1.jpg\",\"eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6_2.jpg\"]", "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "TicketJPGFileNames", "TicketPDFFileName" },
                values: new object[] { "[\"eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6_1.jpg\",\"eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6_2.jpg\"]", "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6.pdf" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "City", "FlatNumber", "HouseNumber", "PhoneNumber", "Street", "ZipCode" },
                values: new object[] { "Warszawa", 14m, 12m, "789456123", "Wesoła", "15-264" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "City", "FlatNumber", "HouseNumber", "PhoneNumber", "Street", "ZipCode" },
                values: new object[] { "Poznań", 31m, 10m, "123456789", "Wiejska", "01-342" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "City", "FlatNumber", "HouseNumber", "PhoneNumber", "Street", "ZipCode" },
                values: new object[] { "Białystok", 21m, 7m, "147852369", "Pogodna", "14-453" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "City", "FlatNumber", "HouseNumber", "PhoneNumber", "Street", "ZipCode" },
                values: new object[] { "Warszawa", 42m, 21m, "147852369", "Słoneczna", "14-453" });

            migrationBuilder.InsertData(
                table: "Festival_Partner",
                columns: new[] { "FestivalId", "PartnerId" },
                values: new object[,]
                {
                    { 1, 7 },
                    { 1, 8 },
                    { 2, 6 },
                    { 2, 7 },
                    { 2, 9 },
                    { 3, 6 },
                    { 3, 8 },
                    { 3, 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Festival_Partner_PartnerId",
                table: "Festival_Partner",
                column: "PartnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Festival_Partner");

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DropColumn(
                name: "City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FlatNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TicketJPGFileNames",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "TicketPDFFileName",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "MaxNumberOfSeats",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "MaxNumberOfSeatsColumns",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "MaxNumberOfSeatsRows",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "NumberOfSeats",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "StageLength",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "StageWidth",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "TotalLength",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "TotalWidth",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Festival");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Event");

            migrationBuilder.DropSequence(
                name: "EventIdSequence");

            migrationBuilder.DropSequence(
                name: "FestivalIdSequence");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Reservation",
                newName: "PaymentTypeId");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "HallRent",
                newName: "PaymentTypeId");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "EventPass",
                newName: "PaymentTypeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftUpdated",
                table: "TicketType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "TicketType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "TicketType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "Ticket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Ticket",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftUpdated",
                table: "SeatType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "SeatType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "SeatType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "Seat",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Seat",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFestivalReservation",
                table: "Reservation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "Reservation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TicketPDFId",
                table: "Reservation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Reservation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Partner",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftUpdated",
                table: "HallType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "HallType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "HallType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "HallRent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "HallRent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "HallRent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "HallRent",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "Hall",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Hall",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Festival",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR [FestivalIdSequence]");

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "Festival",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "Festival",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Festival",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftUpdated",
                table: "EventPassType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "EventPassType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "EventPassType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "EventPass",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreviousEndDate",
                table: "EventPass",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "EventPass",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftUpdated",
                table: "EventCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "EventCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "EventCategory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Event",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR [EventIdSequence]");

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "Event",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "Event",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Event",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "Equipment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Equipment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftUpdated",
                table: "AdditionalServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "AdditionalServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "AdditionalServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LongDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FestivalDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LongDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FestivalDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HallDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaxNumberOfSeats = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: false),
                    MaxNumberOfSeatsColumns = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    MaxNumberOfSeatsRows = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    NumberOfSeats = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: false),
                    StageLength = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: true),
                    StageWidth = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: true),
                    TotalArea = table.Column<decimal>(type: "NUMERIC(5,2)", nullable: false),
                    TotalLength = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: false),
                    TotalWidth = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HallDetails_Hall_Id",
                        column: x => x.Id,
                        principalTable: "Hall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaPatron",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    MediaPatronGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaPatron", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrganizerGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PaymentTypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sponsor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SponsorGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketJPG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketJPG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketPDF",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPDF", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FlatNumber = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: true),
                    HouseNumber = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserData_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Festival_MediaPatron",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    MediaPatronId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival_MediaPatron", x => new { x.FestivalId, x.MediaPatronId });
                    table.ForeignKey(
                        name: "FK_Festival_MediaPatron_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Festival_MediaPatron_MediaPatron_MediaPatronId",
                        column: x => x.MediaPatronId,
                        principalTable: "MediaPatron",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Festival_Organizer",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    OrganizerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival_Organizer", x => new { x.FestivalId, x.OrganizerId });
                    table.ForeignKey(
                        name: "FK_Festival_Organizer_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Festival_Organizer_Organizer_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Organizer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Festival_Sponsor",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    SponsorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival_Sponsor", x => new { x.FestivalId, x.SponsorId });
                    table.ForeignKey(
                        name: "FK_Festival_Sponsor_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Festival_Sponsor_Sponsor_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "Sponsor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation_TicketJPG",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    TicketJPGId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation_TicketJPG", x => new { x.ReservationId, x.TicketJPGId });
                    table.ForeignKey(
                        name: "FK_Reservation_TicketJPG_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_TicketJPG_TicketJPG_TicketJPGId",
                        column: x => x.TicketJPGId,
                        principalTable: "TicketJPG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "AdditionalServices",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 3600L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 10800L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 7200L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 10800L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 3600L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 7200L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 10800L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 9000L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 7200L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 7200L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 7200L, false, null });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 7200L, false, null });

            migrationBuilder.UpdateData(
                table: "EventCategory",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "EventCategory",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "EventCategory",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "EventCategory",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.InsertData(
                table: "EventDetails",
                columns: new[] { "Id", "LongDescription" },
                values: new object[,]
                {
                    { 1, "Koncert Mystic Waves to wyjątkowe wydarzenie muzyczne, które przeniesie Cię w świat atmosferycznych dźwięków. Zespół wystąpi z premierowym materiałem, łączącym elementy muzyki elektronicznej i ambientowej. Koncert odbędzie się w klimatycznej sali, z wykorzystaniem nowoczesnej technologii dźwiękowej i wizualnej, co zapewni niezapomniane przeżycia. Muzycy znani z licznych występów na scenach festiwalowych zaprezentują swoją najnowszą produkcję, która z pewnością oczaruje fanów muzyki eksperymentalnej." },
                    { 2, "Spektakl 'Cień Przeszłość' to wciągająca produkcja teatralna, która porusza tematykę przemijania i refleksji nad przeszłością. Historia opowiada o dwóch postaciach, które spotykają się po latach, by skonfrontować swoje wspomnienia i tajemnice. Reżyseria, mistrzowskie aktorstwo oraz emocjonalna głębia tej sztuki sprawiają, że jest to wydarzenie, które zostanie z widzami na długo. Spektakl jest pełen symboliki i zaskakujących zwrotów akcji, które skłonią do przemyśleń o upływającym czasie." },
                    { 3, "Film 'Królestwo planety małp' to spektakularna produkcja science-fiction, która kontynuuje historię walczących o przetrwanie małp. Akcja filmu toczy się w post-apokaliptycznym świecie, w którym małpy zaczynają dominować nad ludźmi. W filmie zobaczymy dynamiczne sceny bitewne, dramatyczne decyzje bohaterów i wciągającą fabułę, która porusza kwestie moralności i przyszłości ludzkości. Wysoka jakość efektów specjalnych oraz rewelacyjna gra aktorska sprawiają, że jest to jeden z najważniejszych filmów tego roku." },
                    { 4, "Wystawa 'Nowe inspiracje' to wyjątkowa ekspozycja sztuki współczesnej, która gromadzi prace młodych artystów z całego świata. Wystawa skupia się na nowych nurtach artystycznych, które łączą tradycję z nowoczesnością. Można tu zobaczyć prace w różnych mediach, od malarstwa po instalacje interaktywne. Artyści eksperymentują z formą i materiałami, zapraszając widzów do interakcji z dziełami. Jest to przestrzeń, która otwiera umysł na nowe pomysły i inspiracje, zachęcając do refleksji nad współczesnym światem sztuki." },
                    { 5, "Koncert 'New Era' to wyjątkowe wydarzenie muzyczne, które łączy najnowsze trendy w muzyce elektronicznej z klasycznymi brzmieniami. Zespół zaprezentuje utwory z debiutanckiego albumu, który zdobył uznanie wśród krytyków muzycznych. Występ będzie pełen energetycznych melodii i przejmujących dźwięków, które z pewnością poruszą serca słuchaczy. Nowoczesne technologie audio-wizualne, wykorzystywane podczas koncertu, sprawią, że każdy utwór nabierze nowego wymiaru. Warto być częścią tego muzycznego doświadczenia!" },
                    { 6, "Film 'Gladiator' to epicka opowieść o wojowniku, który staje do walki nie tylko o swoje życie, ale także o wolność i sprawiedliwość. Akcja rozgrywa się w starożytnym Rzymie, gdzie główny bohater, Maximus, zostaje zdradzony przez cesarza, a jego rodzina zostaje zamordowana. Maximus trafia do areny, gdzie walczy o przetrwanie i zemstę. Film pełen jest emocjonujących walk, dramatycznych zwrotów akcji i wciągającej fabuły, która porusza tematy honoru, lojalności i zemsty." },
                    { 7, "Wystawa 'Nowa sztuka' to prezentacja młodych artystów, którzy swoją twórczość ukierunkowali na przekroczenie granic tradycyjnej sztuki. Na wystawie można zobaczyć prace w różnorodnych technikach: od klasycznych obrazów po awangardowe instalacje. Artyści bawią się konwencjami, angażując widza w interaktywną formę prezentacji. Wystawa jest zaproszeniem do refleksji nad tym, czym współczesna sztuka może być i jakie emocje potrafi wywołać. To wydarzenie, które pokazuje, jak sztuka może zmieniać nasze postrzeganie świata." },
                    { 8, "Cień Nad Miastem to mroczny thriller teatralny, który przenosi widza w świat pełen tajemniczych wydarzeń, które zaczynają wstrząsać miastem po zmroku. Główna fabuła opowiada historię, w której pozornie spokojne miasto staje się areną niebezpiecznych wydarzeń, których rozwiązanie wciąga nie tylko bohaterów, ale również publiczność. W spektaklu dominują napięcie i atmosfera niepokoju, które potęgują świetnie dobrane efekty wizualne oraz świetne aktorstwo. Każda scena jest przemyślana, a dramaturgia składa się z zagadek i zwrotów akcji, które trzymają widza w ciągłym napięciu. To przedstawienie nie tylko o mrocznych wydarzeniach, ale także o ludzkich emocjach, które rodzą się w sytuacjach kryzysowych, zmieniając życie bohaterów na zawsze. W trakcie spektaklu widzowie będą mieli okazję odkryć tajemnice miasta i skonfrontować się z pytaniem, jak cienka jest granica między rzeczywistością a tym, co wytworzyła wyobraźnia." },
                    { 9, "Jazzowe Brzmienia to wyjątkowy koncert, który przenosi uczestników w świat żywego jazzu. Podczas wydarzenia wystąpią znani artyści jazzowi, którzy zabiorą widzów w podróż po różnorodnych stylach tego gatunku. Atmosfera pełna emocji, improwizacji i magicznych dźwięków sprawia, że każda chwila staje się niezapomnianym doświadczeniem. Koncert jest idealną okazją, by poczuć autentyczną magię jazzu w wyjątkowym wykonaniu na żywo, a publiczność wciągnie się w niepowtarzalny klimat muzycznej ekspresji." },
                    { 10, "Interstellar to epicka podróż w głąb kosmosu, która bada granice ludzkiej wytrzymałości, miłości i poświęcenia. Grupa astronautów, wyruszając na misję w poszukiwaniu nowego domu dla ludzkości, zmierzy się z niewyobrażalnymi wyzwaniami. Film porusza tematykę czasu, przestrzeni i ofiary, ukazując niesamowite krajobrazy kosmiczne oraz ludzką determinację w walce o przetrwanie. Każdy moment to prawdziwa uczta dla zmysłów, a historia miłości i poświęcenia łączy się z naukową fascynacją, tworząc niezapomniane doświadczenie filmowe." },
                    { 11, "Spektakl 'Zatrzymać Przeznaczenie' to głęboka opowieść o postaci, która staje przed obliczem nieuchronnego przeznaczenia. Bohaterowie zmagają się z wyborem, który może zmienić bieg wydarzeń, a ich działania prowadzą do nieodwracalnych konsekwencji. Spektakl skłania do refleksji nad wolną wolą, nieuchronnością losu i odpowiedzialnością za własne wybory. Z pomocą wciągającej dramaturgii oraz poruszających dialogów, spektakl ukazuje, jak delikatna jest granica między tym, co możemy kontrolować, a tym, co jest zapisane w naszych losach." },
                    { 12, "Wystawa 'Nowe Perspektywy' zaprasza do odkrywania sztuki współczesnej z zupełnie nowych punktów widzenia. Artyści wykorzystują różnorodne techniki, od tradycyjnych obrazów po innowacyjne instalacje, które angażują widza w interaktywną formę prezentacji. Każda praca na wystawie to zaproszenie do zastanowienia się nad tym, jak sztuka może zmieniać nasze postrzeganie rzeczywistości. To także próba przekroczenia granic tradycyjnych konwencji, poszukiwania nowych środków wyrazu i głębokiej refleksji nad naturą współczesnej sztuki." }
                });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsUpdated", "PreviousEndDate", "UpdateDate" },
                values: new object[] { false, null, null });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsUpdated", "PreviousEndDate", "UpdateDate" },
                values: new object[] { false, null, null });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsUpdated", "PreviousEndDate", "UpdateDate" },
                values: new object[] { false, null, null });

            migrationBuilder.UpdateData(
                table: "EventPassType",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "EventPassType",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "EventPassType",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "EventPassType",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 2595600L, false, null });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 2685600L, false, null });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Duration", "IsUpdated", "UpdateDate" },
                values: new object[] { 2602800L, false, null });

            migrationBuilder.InsertData(
                table: "FestivalDetails",
                columns: new[] { "Id", "LongDescription" },
                values: new object[,]
                {
                    { 1, "Festiwal Muzyki Hip-Hop to coroczne wydarzenie dedykowane nowoczesnym brzmieniom i eksperymentalnym kompozycjom. Na scenie pojawiają się zarówno uznani artyści, jak i młode talenty, prezentując innowacyjne podejście do muzyki. W programie znajdują się koncerty, warsztaty, spotkania z twórcami oraz panele dyskusyjne dotyczące przyszłości muzyki. Festiwal przyciąga miłośników dźwięków elektronicznych, jazzowych improwizacji, minimalistycznych kompozycji i nowych form ekspresji dźwiękowej. To wyjątkowa okazja, by odkryć najnowsze trendy muzyczne i poszerzyć swoje horyzonty artystyczne." },
                    { 2, "Festiwal Filmowy to wyjątkowe święto kina, które gromadzi pasjonatów filmów z różnych zakątków świata. W repertuarze znajdują się zarówno filmy fabularne, dokumentalne, jak i krótkometrażowe, reprezentujące różne nurty i stylistyki. Organizowane są spotkania z reżyserami, aktorami oraz warsztaty dla młodych filmowców. Każda edycja festiwalu poświęcona jest określonemu tematowi przewodniemu, który inspiruje twórców do dyskusji nad istotnymi kwestiami społecznymi, kulturowymi i artystycznymi. To doskonała okazja, by zobaczyć premiery, odkryć nowe talenty i zanurzyć się w magiczny świat kinematografii." },
                    { 3, "Festiwal Sztuki to wydarzenie celebrujące kreatywność i nieograniczoną ekspresję artystyczną. W ramach festiwalu prezentowane są prace artystów, którzy eksperymentują z formą, kolorem i strukturą, tworząc unikalne dzieła sztuki nowoczesnej. Oprócz wystaw odbywają się warsztaty, podczas których uczestnicy mogą spróbować swoich sił w malarstwie, rzeźbie czy instalacjach multimedialnych. Festiwal to także przestrzeń do rozmów o współczesnych trendach w sztuce, spotkania z artystami oraz wykłady ekspertów. To niepowtarzalna okazja, by odkryć nowe inspiracje i spojrzeć na sztukę z innej perspektywy." }
                });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.InsertData(
                table: "HallDetails",
                columns: new[] { "Id", "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalArea", "TotalLength", "TotalWidth" },
                values: new object[,]
                {
                    { 1, 255m, 15m, 15m, 100m, 4.4m, 7m, 308m, 22m, 14m },
                    { 2, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 3, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 4, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m },
                    { 5, 255m, 15m, 15m, 100m, 4.4m, 7m, 108m, 22m, 14m },
                    { 6, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 7, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 8, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m },
                    { 9, 255m, 15m, 15m, 100m, 4.4m, 7m, 308m, 22m, 14m },
                    { 10, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 11, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m },
                    { 12, 255m, 15m, 15m, 100m, 4.4m, 7m, 308m, 22m, 14m },
                    { 13, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 14, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 15, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m },
                    { 16, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 17, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 18, 255m, 15m, 15m, 100m, 4.4m, 7m, 308m, 22m, 14m },
                    { 19, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 20, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m }
                });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Duration", "IsUpdated", "PaymentDate", "UpdateDate" },
                values: new object[] { 28800L, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Duration", "IsUpdated", "PaymentDate", "UpdateDate" },
                values: new object[] { 14400L, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Duration", "IsUpdated", "PaymentDate", "UpdateDate" },
                values: new object[] { 7200L, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Duration", "IsUpdated", "PaymentDate", "UpdateDate" },
                values: new object[] { 3600L, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "HallType",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "HallType",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "HallType",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "HallType",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "HallType",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "HallType",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.InsertData(
                table: "MediaPatron",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsUpdated", "MediaPatronGuid", "Name", "PhotoName", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, new Guid("ae371ef5-1408-4a99-9df5-c02da3894a64"), "Gazeta Nowoczesna", "gazeta_nowoczesna.png", null },
                    { 2, null, false, false, new Guid("1888ac98-9879-49d8-b5a0-a9bdedf65b79"), "Nowy świat TV", "nowy_swiat.png", null },
                    { 3, null, false, false, new Guid("21721629-b1aa-4fd0-8fce-689d78d6c326"), "Tygodnik Nowiny", "tygodnik_nowiny.png", null }
                });

            migrationBuilder.InsertData(
                table: "Organizer",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsUpdated", "Name", "OrganizerGuid", "PhotoName", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, "EventFlow", new Guid("4656853b-fbf1-4407-8a15-9f60fe367343"), "eventflow.png", null },
                    { 2, null, false, false, "Snowflake", new Guid("175c000d-26c4-4519-b49d-c9d1d7935699"), "snowflake.png", null },
                    { 3, null, false, false, "Aura", new Guid("d044fdef-73a3-4f97-962f-d85e36760f4e"), "aura.png", null }
                });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { "Basel", new Guid("8e0ab6f0-ebbd-4d96-ae19-574b276e985c"), "basel.png" });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { "Aura", new Guid("fa1fefc3-3a92-4162-a5cc-30aafea11323"), "aura.png" });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { "Vision", new Guid("bb243a20-bf87-4534-b9b2-a994004f00c4"), "vision.png" });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { "Snowflake", new Guid("2a1dbe6b-42f3-4fec-ab63-dffba70184b7"), "snowflake.png" });

            migrationBuilder.UpdateData(
                table: "Partner",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "PartnerGuid", "PhotoName" },
                values: new object[] { "Waveless", new Guid("20f68122-1bdf-4881-8ce4-f808701648a5"), "waveless.png" });

            migrationBuilder.InsertData(
                table: "PaymentType",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsUpdated", "Name", "PaymentTypeGuid", "PhotoName", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, "PayU", new Guid("2aa93a32-7232-48e7-a05b-8364c8aafffc"), "", null },
                    { 2, null, false, false, "Karnet", new Guid("1938093d-7427-40e5-a736-c2aa4305dc16"), "", null }
                });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsFestivalReservation", "IsUpdated", "PaymentDate", "TicketPDFId", "UpdateDate" },
                values: new object[] { false, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsFestivalReservation", "IsUpdated", "PaymentDate", "TicketPDFId", "UpdateDate" },
                values: new object[] { false, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsFestivalReservation", "IsUpdated", "PaymentDate", "TicketPDFId", "UpdateDate" },
                values: new object[] { false, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsFestivalReservation", "IsUpdated", "PaymentDate", "TicketPDFId", "UpdateDate" },
                values: new object[] { false, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsFestivalReservation", "IsUpdated", "PaymentDate", "TicketPDFId", "UpdateDate" },
                values: new object[] { true, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsFestivalReservation", "IsUpdated", "PaymentDate", "TicketPDFId", "UpdateDate" },
                values: new object[] { true, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsFestivalReservation", "IsUpdated", "PaymentDate", "TicketPDFId", "UpdateDate" },
                values: new object[] { true, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsFestivalReservation", "IsUpdated", "PaymentDate", "TicketPDFId", "UpdateDate" },
                values: new object[] { true, false, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Seat",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "SeatType",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "SeatType",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "SeatType",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.InsertData(
                table: "Sponsor",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsUpdated", "Name", "PhotoName", "SponsorGuid", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, "Basel", "basel.png", new Guid("423d7369-b70c-4471-97a8-968d001718a7"), null },
                    { 2, null, false, false, "Vision", "vision.png", new Guid("6ba10301-43f6-4207-835c-15f03d7904c7"), null },
                    { 3, null, false, false, "Waveless", "waveless.png", new Guid("ca9f16d2-cfa6-47dc-9aff-a0821ce6bf79"), null }
                });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IsUpdated", "UpdateDate" },
                values: new object[] { false, null });

            migrationBuilder.InsertData(
                table: "TicketJPG",
                columns: new[] { "Id", "FileName", "ReservationGuid" },
                values: new object[,]
                {
                    { 1, "eventflow_bilet_test_53538b58-f885-4f4a-b675-a4aa4063ccf3.jpg", new Guid("53538b58-f885-4f4a-b675-a4aa4063ccf3") },
                    { 2, "eventflow_bilet_test_ed8b9230-223b-4609-8d13-aa6017edad09.jpg", new Guid("ed8b9230-223b-4609-8d13-aa6017edad09") },
                    { 3, "eventflow_bilet_test_f9a076c4-3475-4a28-a60c-6e0e3c03731a.jpg", new Guid("f9a076c4-3475-4a28-a60c-6e0e3c03731a") },
                    { 4, "eventflow_bilet_test_de1d6773-f027-4888-996a-0296e5c52708.jpg", new Guid("de1d6773-f027-4888-996a-0296e5c52708") },
                    { 5, "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd_1.jpg", new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd") },
                    { 6, "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd_2.jpg", new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd") },
                    { 7, "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6_1.jpg", new Guid("806cade1-2685-43dc-8cfc-682fc4229db6") },
                    { 8, "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6_2.jpg", new Guid("806cade1-2685-43dc-8cfc-682fc4229db6") }
                });

            migrationBuilder.InsertData(
                table: "TicketPDF",
                columns: new[] { "Id", "FileName", "ReservationGuid" },
                values: new object[,]
                {
                    { 1, "eventflow_bilet_test_53538b58-f885-4f4a-b675-a4aa4063ccf3.pdf", new Guid("53538b58-f885-4f4a-b675-a4aa4063ccf3") },
                    { 2, "eventflow_bilet_test_ed8b9230-223b-4609-8d13-aa6017edad09.pdf", new Guid("ed8b9230-223b-4609-8d13-aa6017edad09") },
                    { 3, "eventflow_bilet_test_f9a076c4-3475-4a28-a60c-6e0e3c03731a.pdf", new Guid("f9a076c4-3475-4a28-a60c-6e0e3c03731a") },
                    { 4, "eventflow_bilet_test_de1d6773-f027-4888-996a-0296e5c52708.pdf", new Guid("de1d6773-f027-4888-996a-0296e5c52708") },
                    { 5, "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd.pdf", new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd") },
                    { 6, "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6.pdf", new Guid("806cade1-2685-43dc-8cfc-682fc4229db6") }
                });

            migrationBuilder.UpdateData(
                table: "TicketType",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "TicketType",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.UpdateData(
                table: "TicketType",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsSoftUpdated", "IsUpdated", "UpdateDate" },
                values: new object[] { false, false, null });

            migrationBuilder.InsertData(
                table: "UserData",
                columns: new[] { "Id", "City", "FlatNumber", "HouseNumber", "PhoneNumber", "Street", "ZipCode" },
                values: new object[,]
                {
                    { "1", "Warszawa", 14m, 12m, "789456123", "Wesoła", "15-264" },
                    { "2", "Poznań", 31m, 10m, "123456789", "Wiejska", "01-342" },
                    { "3", "Białystok", 21m, 7m, "147852369", "Pogodna", "14-453" },
                    { "4", "Warszawa", 42m, 21m, "147852369", "Słoneczna", "14-453" }
                });

            migrationBuilder.InsertData(
                table: "Festival_MediaPatron",
                columns: new[] { "FestivalId", "MediaPatronId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Festival_Organizer",
                columns: new[] { "FestivalId", "OrganizerId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Festival_Sponsor",
                columns: new[] { "FestivalId", "SponsorId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Reservation_TicketJPG",
                columns: new[] { "ReservationId", "TicketJPGId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 5, 6 },
                    { 6, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 7, 8 },
                    { 8, 7 },
                    { 8, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_PaymentTypeId",
                table: "Reservation",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TicketPDFId",
                table: "Reservation",
                column: "TicketPDFId");

            migrationBuilder.CreateIndex(
                name: "IX_HallRent_PaymentTypeId",
                table: "HallRent",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPass_PaymentTypeId",
                table: "EventPass",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Festival_MediaPatron_MediaPatronId",
                table: "Festival_MediaPatron",
                column: "MediaPatronId");

            migrationBuilder.CreateIndex(
                name: "IX_Festival_Organizer_OrganizerId",
                table: "Festival_Organizer",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Festival_Sponsor_SponsorId",
                table: "Festival_Sponsor",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TicketJPG_TicketJPGId",
                table: "Reservation_TicketJPG",
                column: "TicketJPGId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventDetails_Id",
                table: "Event",
                column: "Id",
                principalTable: "EventDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventPass_PaymentType_PaymentTypeId",
                table: "EventPass",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Festival_FestivalDetails_Id",
                table: "Festival",
                column: "Id",
                principalTable: "FestivalDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HallRent_PaymentType_PaymentTypeId",
                table: "HallRent",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_PaymentType_PaymentTypeId",
                table: "Reservation",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_TicketPDF_TicketPDFId",
                table: "Reservation",
                column: "TicketPDFId",
                principalTable: "TicketPDF",
                principalColumn: "Id");
        }
    }
}
