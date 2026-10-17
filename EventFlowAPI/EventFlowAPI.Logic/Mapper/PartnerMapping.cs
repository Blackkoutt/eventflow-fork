using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class PartnerMapping
{
    public static Partner ToEntity(this PartnerRequestDto source) => source is null ? null! : source.ApplyTo(new Partner());

    public static Partner ApplyTo(this PartnerRequestDto source, Partner target)
    {
        target.Kind = source.Kind ?? default;
        target.Name = source.Name;
        return target;
    }

    public static Partner ToEntity(this UpdatePartnerRequestDto source) => source is null ? null! : source.ApplyTo(new Partner());

    public static Partner ApplyTo(this UpdatePartnerRequestDto source, Partner target)
    {
        target.Name = source.Name;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static PartnerResponseDto? ToResponse(this Partner? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static PartnerResponseDto? ToResponse(this Partner? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<PartnerResponseDto>(source, out var existing)) return existing;
        var target = new PartnerResponseDto();
        context.Add(source, target);
        target.Kind = source.Kind;
        target.Name = source.Name;
        target.PhotoName = source.PhotoName;
        target.Id = source.Id;
        return target;
    }
}
