using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Identity.Services.Services;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces.Configuration.HallConfiguration;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces.Configuration.PassConfiguration;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces.Configuration.TicketConfiguration;
using EventFlowAPI.Logic.Services.OtherServices.Services;
using EventFlowAPI.Logic.Services.OtherServices.Services.Configuration.HallConfiguration;
using EventFlowAPI.Logic.Services.OtherServices.Services.Configuration.PassConfiguration;
using EventFlowAPI.Logic.Services.OtherServices.Services.Configuration.TicketConfiguration;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Extensions
{
    public static class BuilderExtentionsServices
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
        public static void AddApplicationCRUDServices(this IServiceCollection services)
        {
            services.AddScoped<IAdditionalServicesService, AdditionalServicesService>();
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IEventCategoryService, EventCategoryService>();
            services.AddScoped<IEventPassService, EventPassService>();
            services.AddScoped<IEventPassTypeService, EventPassTypeService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IFestivalService, FestivalService>();
            services.AddScoped<IHallRentService, HallRentService>();
            services.AddScoped<IHallService, HallService>();
            services.AddScoped<IHallTypeService, HallTypeService>();
            services.AddScoped<IMediaPatronService, MediaPatronService>();
            services.AddScoped<IOrganizerService, OrganizerService>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<ISeatTypeService, SeatTypeService>();
            services.AddScoped<ISponsorService, SponsorService>();
            services.AddScoped<ITicketTypeService, TicketTypeService>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IFAQService, FAQService>();
        }
        public static void AddApplicationAuthServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IFacebookAuthService, FacebookAuthService>();
            services.AddSingleton<IJWTGeneratorService, JWTGeneratorService>();
        }
        public static void AddApplicationOtherServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IJPGCreatorService, JPGCreatorService>();
            services.AddScoped<IQRCodeGeneratorService, QRCodeGeneratorService>();
            services.AddScoped<IEventTicketConfiguration, EventTicketConfiguration>();
            services.AddScoped<IFestivalTicketConfiguration, FestivalTicketConfiguration>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IPdfBuilderService, PdfBuilderService>();
            services.AddScoped<IHtmlRendererService, HtmlRendererService>();
            services.AddSingleton<IBlobService, BlobService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEventPassConfiguration, EventPassConfiguration>();
            services.AddScoped<ICollisionCheckerService, CollisionCheckerService>();
            services.AddScoped<IHallSeatsConfiguration, HallSeatsConfiguration>();
            services.AddScoped<ICopyMakerService, CopyMakerService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IPlotService, PlotService>();
        }
    }
}
