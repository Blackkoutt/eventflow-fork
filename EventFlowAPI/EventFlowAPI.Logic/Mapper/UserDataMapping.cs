using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class UserDataMapping
{
    public static UserData ToEntity(this UserDataRequestDto source) => source is null ? null! : source.ApplyTo(new UserData());

    public static UserData ApplyTo(this UserDataRequestDto source, UserData target)
    {
        target.Street = source.Street;
        target.HouseNumber = source.HouseNumber;
        target.FlatNumber = source.FlatNumber;
        target.City = source.City;
        target.ZipCode = source.ZipCode;
        target.PhoneNumber = source.PhoneNumber;
        return target;
    }

    public static UserData ToEntity(this UpdateUserDataRequestDto source) => source is null ? null! : source.ApplyTo(new UserData());

    public static UserData ApplyTo(this UpdateUserDataRequestDto source, UserData target)
    {
        target.Street = source.Street;
        target.HouseNumber = source.HouseNumber;
        target.FlatNumber = source.FlatNumber;
        target.City = source.City;
        target.ZipCode = source.ZipCode;
        target.PhoneNumber = source.PhoneNumber;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static UserDataResponseDto? ToResponse(this UserData? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static UserDataResponseDto? ToResponse(this UserData? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<UserDataResponseDto>(source, out var existing)) return existing;
        var target = new UserDataResponseDto();
        context.Add(source, target);
        target.Street = source.Street!;
        target.HouseNumber = source.HouseNumber ?? 0;
        target.FlatNumber = source.FlatNumber;
        target.City = source.City!;
        target.ZipCode = source.ZipCode!;
        target.PhoneNumber = source.PhoneNumber!;
        target.Id = Convert.ToInt32(source.Id);
        return target;
    }
}
