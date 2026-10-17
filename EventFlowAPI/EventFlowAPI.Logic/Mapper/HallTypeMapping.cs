using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class HallTypeMapping
{
    public static HallType ToEntity(this HallTypeRequestDto source) => source is null ? null! : source.ApplyTo(new HallType());

    public static HallType ApplyTo(this HallTypeRequestDto source, HallType target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        return target;
    }

    public static HallType ToEntity(this UpdateHallTypeRequestDto source) => source is null ? null! : source.ApplyTo(new HallType());

    public static HallType ApplyTo(this UpdateHallTypeRequestDto source, HallType target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static HallTypeResponseDto? ToResponse(this HallType? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static HallTypeResponseDto? ToResponse(this HallType? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<HallTypeResponseDto>(source, out var existing)) return existing;
        var target = new HallTypeResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.Description = source.Description;
        target.PhotoName = source.PhotoName;
        target.Equipments = source.Equipments?.Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Id = source.Id;
        return target;
    }
}
