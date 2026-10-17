using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class FAQMapping
{
    public static FAQ ToEntity(this FAQRequestDto source) => source is null ? null! : source.ApplyTo(new FAQ());

    public static FAQ ApplyTo(this FAQRequestDto source, FAQ target)
    {
        target.Question = source.Question;
        target.Answer = source.Answer;
        return target;
    }

    public static FAQ ToEntity(this UpdateFAQRequestDto source) => source is null ? null! : source.ApplyTo(new FAQ());

    public static FAQ ApplyTo(this UpdateFAQRequestDto source, FAQ target)
    {
        target.Question = source.Question;
        target.Answer = source.Answer;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static FAQResponseDto? ToResponse(this FAQ? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static FAQResponseDto? ToResponse(this FAQ? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<FAQResponseDto>(source, out var existing)) return existing;
        var target = new FAQResponseDto();
        context.Add(source, target);
        target.Question = source.Question;
        target.Answer = source.Answer;
        target.Id = source.Id;
        return target;
    }
}
