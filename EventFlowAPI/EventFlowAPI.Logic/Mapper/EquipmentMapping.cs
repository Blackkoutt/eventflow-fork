using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class EquipmentMapping
{
    public static Equipment ToEntity(this EquipmentRequestDto source) => source is null ? null! : source.ApplyTo(new Equipment());

    public static Equipment ApplyTo(this EquipmentRequestDto source, Equipment target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        return target;
    }

    public static Equipment ToEntity(this UpdateEquipmentRequestDto source) => source is null ? null! : source.ApplyTo(new Equipment());

    public static Equipment ApplyTo(this UpdateEquipmentRequestDto source, Equipment target)
    {
        target.Description = source.Description;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static EquipmentResponseDto? ToResponse(this Equipment? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static EquipmentResponseDto? ToResponse(this Equipment? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<EquipmentResponseDto>(source, out var existing)) return existing;
        var target = new EquipmentResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.Description = source.Description;
        target.Id = source.Id;
        return target;
    }
}
