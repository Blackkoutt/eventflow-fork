using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;

namespace EventFlowAPI.Logic.Mapper;

public static class FestivalDetailsMapping
{
    public static FestivalDetails ToEntity(this FestivalDetailsRequestDto source) => source is null ? null! : source.ApplyTo(new FestivalDetails());

    public static FestivalDetails ApplyTo(this FestivalDetailsRequestDto source, FestivalDetails target)
    {
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static FestivalDetailsResponseDto? ToResponse(this FestivalDetails? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static FestivalDetailsResponseDto? ToResponse(this FestivalDetails? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<FestivalDetailsResponseDto>(source, out var existing)) return existing;
        var target = new FestivalDetailsResponseDto();
        context.Add(source, target);
        target.LongDescription = source.LongDescription;
        target.Id = source.Id;
        return target;
    }
}
