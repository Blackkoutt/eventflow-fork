using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class AdditionalServicesMapping
{
    public static AdditionalServices ToEntity(this AdditionalServicesRequestDto source) => source is null ? null! : source.ApplyTo(new AdditionalServices());

    public static AdditionalServices ApplyTo(this AdditionalServicesRequestDto source, AdditionalServices target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        target.Price = source.Price;
        return target;
    }

    public static AdditionalServices ToEntity(this UpdateAdditionalServicesRequestDto source) => source is null ? null! : source.ApplyTo(new AdditionalServices());

    public static AdditionalServices ApplyTo(this UpdateAdditionalServicesRequestDto source, AdditionalServices target)
    {
        target.Description = source.Description;
        target.Price = source.Price;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static AdditionalServicesResponseDto? ToResponse(this AdditionalServices? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static AdditionalServicesResponseDto? ToResponse(this AdditionalServices? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<AdditionalServicesResponseDto>(source, out var existing)) return existing;
        var target = new AdditionalServicesResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.Price = source.Price;
        target.Description = source.Description;
        target.Id = source.Id;
        return target;
    }
}
