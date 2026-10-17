using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class OrganizerMapping
{
    public static Organizer ToEntity(this OrganizerRequestDto source) => source is null ? null! : source.ApplyTo(new Organizer());

    public static Organizer ApplyTo(this OrganizerRequestDto source, Organizer target)
    {
        target.Name = source.Name;
        return target;
    }

    public static Organizer ToEntity(this UpdateOrganizerRequestDto source) => source is null ? null! : source.ApplyTo(new Organizer());

    public static Organizer ApplyTo(this UpdateOrganizerRequestDto source, Organizer target)
    {
        target.Name = source.Name;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static OrganizerResponseDto? ToResponse(this Organizer? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static OrganizerResponseDto? ToResponse(this Organizer? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<OrganizerResponseDto>(source, out var existing)) return existing;
        var target = new OrganizerResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.PhotoName = source.PhotoName;
        target.Id = source.Id;
        return target;
    }
}
