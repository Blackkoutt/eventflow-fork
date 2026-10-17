using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class SponsorMapping
{
    public static Sponsor ToEntity(this SponsorRequestDto source) => source is null ? null! : source.ApplyTo(new Sponsor());

    public static Sponsor ApplyTo(this SponsorRequestDto source, Sponsor target)
    {
        target.Name = source.Name;
        return target;
    }

    public static Sponsor ToEntity(this UpdateSponsorRequestDto source) => source is null ? null! : source.ApplyTo(new Sponsor());

    public static Sponsor ApplyTo(this UpdateSponsorRequestDto source, Sponsor target)
    {
        target.Name = source.Name;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static SponsorResponseDto? ToResponse(this Sponsor? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static SponsorResponseDto? ToResponse(this Sponsor? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<SponsorResponseDto>(source, out var existing)) return existing;
        var target = new SponsorResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.PhotoName = source.PhotoName;
        target.Id = source.Id;
        return target;
    }
}
