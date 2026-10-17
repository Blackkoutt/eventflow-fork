using AutoMapper;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Identity.DTO.RequestDto;
using EventFlowAPI.Logic.Identity.DTO.ResponseDto;

namespace EventFlowAPI.Logic.Mapper.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AdditionalServices, AdditionalServices>();
            CreateMap<AdditionalServicesRequestDto, AdditionalServices>();
            CreateMap<UpdateAdditionalServicesRequestDto, AdditionalServices>();
            CreateMap<AdditionalServices, AdditionalServicesResponseDto>();

            CreateMap<Equipment, Equipment>();
            CreateMap<EquipmentRequestDto, Equipment>();
            CreateMap<UpdateEquipmentRequestDto, Equipment>();
            CreateMap<Equipment, EquipmentResponseDto>();

            CreateMap<EventCategory, EventCategory>();
            CreateMap<EventCategoryRequestDto, EventCategory>();
            CreateMap<UpdateEventCategoryRequestDto, EventCategory>();
            CreateMap<EventCategory, EventCategoryResponseDto>();

            CreateMap<Event, EventDetailsResponseDto>();

            CreateMap<EventPass, EventPass>();
            CreateMap<EventPassRequestDto, EventPass>();
            CreateMap<UpdateEventPassRequestDto, EventPass>();
            CreateMap<EventPass, EventPassResponseDto>();

            CreateMap<EventPassType, EventPassType>();
            CreateMap<EventPassTypeRequestDto, EventPassType>();
            CreateMap<UpdateEventPassTypeRequestDto, EventPassType>();
            CreateMap<EventPassType, EventPassTypeResponseDto>();

            CreateMap<Event, Event>();
            CreateMap<UpdateEventRequestDto, Event>()
                .ForMember(dest => dest.LongDescription, opt => opt.Ignore());
            CreateMap<EventRequestDto, Event>()
                .ForMember(dest => dest.LongDescription, opt => opt.Ignore());
            CreateMap<Event, EventResponseDto>();

            CreateMap<Ticket, Ticket>();
            CreateMap<TicketRequestDto, Ticket>();
            CreateMap<Ticket, TicketResponseDto>();

            CreateMap<Festival, FestivalDetailsResponseDto>();

            CreateMap<Festival, Festival>();
            CreateMap<UpdateFestivalRequestDto, Festival>()
                .ForMember(dest => dest.LongDescription, opt => opt.Ignore());
            CreateMap<FestivalRequestDto, Festival>()
                .ForMember(dest => dest.LongDescription, opt => opt.Ignore());
            CreateMap<Festival, FestivalResponseDto>();

            CreateMap<HallRent, HallRent>();
            CreateMap<HallRentRequestDto, HallRent>();
            CreateMap<UpdateHallRentRequestDto, HallRent>();
            CreateMap<HallRent, HallRentResponseDto>()
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.RentDate));

            CreateMap<Hall, Hall>();
            CreateMap<HallRequestDto, Hall>();
            CreateMap<UpdateHallRequestDto, Hall>();
            CreateMap<EventHallRequestDto, Hall>();
            CreateMap<HallRent_HallRequestDto, Hall>();
            CreateMap<Hall, HallResponseDto>();

            CreateMap<HallDetailsRequestDto, Hall>();
            CreateMap<EventHallDetailsRequestDto, Hall>();
            CreateMap<HallRent_HallDetailsRequestDto, Hall>();
            CreateMap<Hall, HallDetailsResponseDto>();

            CreateMap<HallType, HallType>();
            CreateMap<HallTypeRequestDto, HallType>();
            CreateMap<UpdateHallTypeRequestDto, HallType>();
            CreateMap<HallType, HallTypeResponseDto>();



            CreateMap<PaymentTypeInfo, PaymentTypeResponseDto>();
            CreateMap<PaymentType, PaymentTypeResponseDto>().ConvertUsing(src => src.AsDto());

            CreateMap<Reservation, Reservation>();
            CreateMap<ReservationRequestDto, Reservation>();
            CreateMap<UpdateReservationRequestDto, Reservation>();
            CreateMap<Reservation, ReservationResponseDto>()
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.ReservationDate));

            CreateMap<Seat, Seat>();
            CreateMap<SeatRequestDto, Seat>();
            CreateMap<Seat, SeatResponseDto>();

            CreateMap<SeatType, SeatType>();
            CreateMap<SeatTypeRequestDto, SeatType>();
            CreateMap<UpdateSeatTypeRequestDto, SeatType>();
            CreateMap<SeatType, SeatTypeResponseDto>();


            CreateMap<News, News>();
            CreateMap<NewsRequestDto, News>();
            CreateMap<UpdateNewsRequestDto, News>();
            CreateMap<News, NewsResponseDto>();

            CreateMap<Partner, Partner>();
            CreateMap<PartnerRequestDto, Partner>();
            CreateMap<UpdatePartnerRequestDto, Partner>();
            CreateMap<Partner, PartnerResponseDto>();

            CreateMap<FAQ, FAQ>();
            CreateMap<FAQRequestDto, FAQ>();
            CreateMap<UpdateFAQRequestDto, FAQ>();
            CreateMap<FAQ, FAQResponseDto>();

            CreateMap<TicketType, TicketType>();
            CreateMap<TicketTypeRequestDto, TicketType>();
            CreateMap<UpdateTicketTypeRequestDto, TicketType>();
            CreateMap<TicketType, TicketTypeResponseDto>();

            CreateMap<UserDataRequestDto, User>();
            CreateMap<UpdateUserDataRequestDto, User>();
            CreateMap<User, UserDataResponseDto>();

            CreateMap<User, User>();
            CreateMap<UserRegisterRequestDto, User>();
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.UserData, opt => opt.MapFrom(src => src.HasUserData ? src : null));
        }
    }
}
