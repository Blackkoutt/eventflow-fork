using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class MediaPatronMapping
{
    public static MediaPatron ToEntity(this MediaPatronRequestDto source) => source is null ? null! : source.ApplyTo(new MediaPatron());

    public static MediaPatron ApplyTo(this MediaPatronRequestDto source, MediaPatron target)
    {
        target.Name = source.Name;
        return target;
    }

    public static MediaPatron ToEntity(this UpdateMediaPatronRequestDto source) => source is null ? null! : source.ApplyTo(new MediaPatron());

    public static MediaPatron ApplyTo(this UpdateMediaPatronRequestDto source, MediaPatron target)
    {
        target.Name = source.Name;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static MediaPatronResponseDto? ToResponse(this MediaPatron? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static MediaPatronResponseDto? ToResponse(this MediaPatron? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<MediaPatronResponseDto>(source, out var existing)) return existing;
        var target = new MediaPatronResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.PhotoName = source.PhotoName;
        target.Id = source.Id;
        return target;
    }
}
