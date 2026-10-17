using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class NewsMapping
{
    public static News ToEntity(this NewsRequestDto source) => source is null ? null! : source.ApplyTo(new News());

    public static News ApplyTo(this NewsRequestDto source, News target)
    {
        target.Title = source.Title;
        target.ShortDescription = source.ShortDescription;
        target.LongDescription = source.LongDescription;
        return target;
    }

    public static News ToEntity(this UpdateNewsRequestDto source) => source is null ? null! : source.ApplyTo(new News());

    public static News ApplyTo(this UpdateNewsRequestDto source, News target)
    {
        target.Title = source.Title;
        target.ShortDescription = source.ShortDescription;
        target.LongDescription = source.LongDescription;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static NewsResponseDto? ToResponse(this News? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static NewsResponseDto? ToResponse(this News? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<NewsResponseDto>(source, out var existing)) return existing;
        var target = new NewsResponseDto();
        context.Add(source, target);
        target.Title = source.Title;
        target.PublicationDate = source.PublicationDate;
        target.ShortDescription = source.ShortDescription;
        target.LongDescription = source.LongDescription;
        target.PhotoName = source.PhotoName;
        target.Id = source.Id;
        return target;
    }
}
