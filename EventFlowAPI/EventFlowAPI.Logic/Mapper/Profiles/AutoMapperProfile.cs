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

            //CreateMap<EventDetailsRequestDto, EventDetails>();
            CreateMap<EventDetails, EventDetails>();
            CreateMap<EventDetails, EventDetailsResponseDto>();

            CreateMap<EventPass, EventPass>();
            CreateMap<EventPassRequestDto, EventPass>();
            CreateMap<UpdateEventPassRequestDto, EventPass>();
            CreateMap<EventPass, EventPassResponseDto>();

            CreateMap<EventPassType, EventPassType>();
            CreateMap<EventPassTypeRequestDto, EventPassType>();
            CreateMap<UpdateEventPassTypeRequestDto, EventPassType>();
            CreateMap<EventPassType, EventPassTypeResponseDto>();

            CreateMap<Event, Event>();
            CreateMap<UpdateEventRequestDto, Event>();
            CreateMap<EventRequestDto, Event>();
            CreateMap<Event, EventResponseDto>();

            CreateMap<Ticket, Ticket>();
            CreateMap<TicketRequestDto, Ticket>();
            CreateMap<Ticket, TicketResponseDto>();

            CreateMap<FestivalDetails, FestivalDetails>();
            CreateMap<FestivalDetailsRequestDto, FestivalDetails>();
            CreateMap<FestivalDetails, FestivalDetailsResponseDto>();

            CreateMap<Festival, Festival>();
            CreateMap<UpdateFestivalRequestDto, Festival>();
            CreateMap<FestivalRequestDto, Festival>();
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

            CreateMap<HallDetails, HallDetails>();
            CreateMap<HallDetailsRequestDto, HallDetails>();
            CreateMap<EventHallDetailsRequestDto, HallDetails>();
            CreateMap<HallRent_HallDetailsRequestDto, HallDetails>();
            CreateMap<HallDetails, HallDetailsResponseDto>();

            CreateMap<HallType, HallType>();
            CreateMap<HallTypeRequestDto, HallType>();
            CreateMap<UpdateHallTypeRequestDto, HallType>();
            CreateMap<HallType, HallTypeResponseDto>();

            CreateMap<MediaPatron, MediaPatron>();
            CreateMap<MediaPatronRequestDto, MediaPatron>();
            CreateMap<UpdateMediaPatronRequestDto, MediaPatron>();
            CreateMap<MediaPatron, MediaPatronResponseDto>();

            CreateMap<Organizer, Organizer>();
            CreateMap<OrganizerRequestDto, Organizer>();
            CreateMap<UpdateOrganizerRequestDto, Organizer>();
            CreateMap<Organizer, OrganizerResponseDto>();

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

            CreateMap<Sponsor, Sponsor>();
            CreateMap<SponsorRequestDto, Sponsor>();
            CreateMap<UpdateSponsorRequestDto, Sponsor>();
            CreateMap<Sponsor, SponsorResponseDto>();

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

            CreateMap<UserData, UserData>();
            CreateMap<UserDataRequestDto, UserData>();
            CreateMap<UpdateUserDataRequestDto, UserData>();
            CreateMap<UserData, UserDataResponseDto>();

            CreateMap<User, User>();
            CreateMap<UserRegisterRequestDto, User>();
            CreateMap<User, UserResponseDto>();
        }
    }
}
