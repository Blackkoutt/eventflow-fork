using EventFlowAPI.DB.Entities.Enums;
using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class FestivalMapping
{
    public static Festival ToEntity(this UpdateFestivalRequestDto source) => source is null ? null! : source.ApplyTo(new Festival());

    public static Festival ApplyTo(this UpdateFestivalRequestDto source, Festival target)
    {
        target.Name = source.Name;
        target.ShortDescription = source.ShortDescription;
        return target;
    }

    public static Festival ToEntity(this FestivalRequestDto source) => source is null ? null! : source.ApplyTo(new Festival());

    public static Festival ApplyTo(this FestivalRequestDto source, Festival target)
    {
        target.Name = source.Name;
        target.ShortDescription = source.ShortDescription;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static FestivalResponseDto? ToResponse(this Festival? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static FestivalResponseDto? ToResponse(this Festival? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<FestivalResponseDto>(source, out var existing)) return existing;
        var target = new FestivalResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.ShortDescription = source.ShortDescription;
        target.StartDate = source.StartDate;
        target.EndDate = source.EndDate;
        target.Duration = source.Duration;
        target.PhotoName = source.PhotoName;
        target.Details = source.Details.ToResponse(context);
        target.Events = source.Events?.Select(item => item.ToResponse(context)).ToList() ?? [];
        target.MediaPatrons = source.Partners?.Where(p => p.Kind == PartnerKind.MediaPatron).Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Organizers = source.Partners?.Where(p => p.Kind == PartnerKind.Organizer).Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Sponsors = source.Partners?.Where(p => p.Kind == PartnerKind.Sponsor).Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Tickets = source.Tickets?.Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Id = source.Id;
        return target;
    }
}
